using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CSVLoader
{
    public Dictionary<string, string> LoadLanguage(TextAsset csvFile, string language)
    {
        var translations = new Dictionary<string, string>();

        string[] lines = csvFile.text.Split('\n');

        if (lines.Length == 0)
            return translations;

        string[] headers = ParseLine(lines[0]);

        int languageIndex = GetHeaderIndex(headers, language);

        if (languageIndex == -1)
        {
            Debug.LogError($"Language '{language}' was not found in the CSV.");
            return translations;
        }

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            string[] values = ParseLine(lines[i]);

            if (values.Length <= languageIndex)
            {
                Debug.LogWarning($"Invalid localization line: {lines[i]}");
                continue;
            }

            string id = values[0];
            string text = values[languageIndex];

            translations[id] = text;
        }

        return translations;
    }

    private string[] ParseLine(string line)
    {
        var values = new List<string>();
        var currentValue = new StringBuilder();

        bool insideQuotes = false;

        foreach (char character in line)
        {
            if (character == '"')
            {
                insideQuotes = !insideQuotes;
                continue;
            }

            if (character == ',' && !insideQuotes)
            {
                values.Add(currentValue.ToString().Trim());
                currentValue.Clear();
                continue;
            }

            currentValue.Append(character);
        }

        values.Add(currentValue.ToString().Trim());

        return values.ToArray();
    }

    private int GetHeaderIndex(string[] headers, string language)
    {
        for (int i = 0; i < headers.Length; i++)
        {
            if (headers[i].Equals(language, System.StringComparison.OrdinalIgnoreCase))
                return i;
        }

        return -1;
    }
}
