using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum FlowColor
{
    None,
    Blue,
    Red,
    Yellow,
    Orange,
    Green
}



public class FlowGame : MonoBehaviour
{
    [System.Serializable]
    private class ColorLine
    {
        public FlowColor Color;
        public LineRenderer Line;
    }

    [SerializeField] private Camera _camera;
    [SerializeField] private ColorLine[] _colorLines;

    private readonly Dictionary<FlowColor, List<FlowCell>> _paths = new();
    private readonly Dictionary<FlowColor, LineRenderer> _lines = new();
    private readonly HashSet<FlowColor> _completedColors = new();

    private List<FlowCell> _currentPath;
    private FlowColor _currentColor;

    private bool _isDrawing;
    public Action Finish;

    private void Awake()
    {
        foreach (ColorLine colorLine in _colorLines)
        {
            _paths.Add(colorLine.Color, new List<FlowCell>());
            _lines.Add(colorLine.Color, colorLine.Line);
        }
    }

    public void UpdateFlowGame()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
           
            StartDrawing();
        }

        if (Mouse.current.leftButton.isPressed && _isDrawing)
            ContinueDrawing();

        if (Mouse.current.leftButton.wasReleasedThisFrame)
            StopDrawing();
    }

    private void StartDrawing()
    {
        FlowCell cell = GetCellUnderMouse();

        if (cell == null)
            return;

        // Primero comprobamos si esta celda ya pertenece
        // a alguno de nuestros caminos.
        FlowColor pathColor = GetPathColor(cell);

        if (pathColor != FlowColor.None)
        {
            _currentColor = pathColor;
            _currentPath = _paths[_currentColor];

            // Ya no consideramos el color completado porque
            // vamos a modificar su camino.
            _completedColors.Remove(_currentColor);

            // Eliminamos todo lo que estaba después de la
            // celda donde hicimos click.
            int index = _currentPath.IndexOf(cell);

            _currentPath.RemoveRange(
                index + 1,
                _currentPath.Count - index - 1
            );

            _isDrawing = true;

            UpdateLine();

            return;
        }

        // Si no pertenece a ningún path, solamente puede
        // comenzar si es un punto.
        if (cell.FlowColor == FlowColor.None)
            return;

        _currentColor = cell.FlowColor;
        _currentPath = _paths[_currentColor];

        _completedColors.Remove(_currentColor);

        _currentPath.Clear();
        _currentPath.Add(cell);

        _isDrawing = true;

        UpdateLine();
    }

    private FlowColor GetPathColor(FlowCell cell)
    {
        foreach (KeyValuePair<FlowColor, List<FlowCell>> path in _paths)
        {
            if (path.Value.Contains(cell))
                return path.Key;
        }

        return FlowColor.None;
    }

    private void ContinueDrawing()
    {
        FlowCell cell = GetCellUnderMouse();

        if (cell == null)
            return;

        FlowCell previousCell = _currentPath[^1];

        if (cell == previousCell)
            return;

        if (!IsNeighbor(previousCell, cell))
            return;

        // No puedes entrar en el punto de otro color.
        if (cell.FlowColor != FlowColor.None &&
            cell.FlowColor != _currentColor)
        {
            return;
        }

        // Volver una celda atrás.
        int previousIndex = _currentPath.Count - 2;

        if (previousIndex >= 0 &&
            cell == _currentPath[previousIndex])
        {
            _currentPath.RemoveAt(_currentPath.Count - 1);

            UpdateLine();

            return;
        }

        // No puedes volver a una celda anterior que no sea
        // la inmediatamente anterior.
        if (_currentPath.Contains(cell))
            return;

        // No puedes atravesar el camino de otro color.
        if (IsOccupiedByOtherColor(cell))
            return;

        _currentPath.Add(cell);

        UpdateLine();

        CheckCompletion(cell);
    }

    private void StopDrawing()
    {
        _isDrawing = false;
        _currentPath = null;
    }

    private bool IsNeighbor(FlowCell current, FlowCell next)
    {
        Vector2Int difference = next.Position - current.Position;

        return Mathf.Abs(difference.x) +
               Mathf.Abs(difference.y) == 1;
    }

    private bool IsOccupiedByOtherColor(FlowCell cell)
    {
        foreach (KeyValuePair<FlowColor, List<FlowCell>> path in _paths)
        {
            if (path.Key == _currentColor)
                continue;

            if (path.Value.Contains(cell))
                return true;
        }

        return false;
    }

    private void CheckCompletion(FlowCell cell)
    {
        // No es un punto.
        if (cell.FlowColor == FlowColor.None)
            return;

        // No es nuestro color.
        if (cell.FlowColor != _currentColor)
            return;

        // Llegamos al otro extremo del mismo color.
        _completedColors.Add(_currentColor);

        Debug.Log($"{_currentColor} completado.");

        CheckWin();
    }

    private void CheckWin()
    {
        if (_completedColors.Count != _paths.Count)
            return;

        Debug.Log("¡GANASTE!");
        Finish?.Invoke();
    }

    private FlowCell GetCellUnderMouse()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Ray ray = _camera.ScreenPointToRay(mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit))
            return null;

        return hit.collider.GetComponent<FlowCell>();
    }

    private void UpdateLine()
    {
        LineRenderer line = _lines[_currentColor];

        line.positionCount = _currentPath.Count;

        for (int i = 0; i < _currentPath.Count; i++)
        {
            line.SetPosition(
                i,
                _currentPath[i].transform.localPosition
            );
        }
    }
}