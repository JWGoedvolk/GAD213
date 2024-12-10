using UnityEngine;
using System.Text.RegularExpressions;

public static class GoogleDriveHelper
{
    public static string ConvertToDirectDownloadLink(string rawLink)
    {
        Match match = Regex.Match(rawLink, @"(?:drive\.google\.com/.*?\/d\/|id=)([a-zA-Z0-9_-]+)");
        string result = string.Empty;
        if (match.Success)
        {
            string id = match.Groups[1].Value;
            result = $"https://drive.google.com/uc?export=download&id={id}";
        }
        else
        {
            Debug.LogError($"Invalid Google Drive Link provided: {rawLink}");
        }

        return result;
    }

    public static string GetIDFromLink(string link)
    {
        Match match = Regex.Match(link, @"(?:drive\.google\.com/.*?\/d\/|id=)([a-zA-Z0-9_-]+)");
        string result = string.Empty;
        if (match.Success)
        {
            result = match.Groups[1].Value;
        }
        else
        {
            Debug.LogError($"Invalid Google Drive Link provided: {link}");
        }

        return result;
    }
}
