using System.Text.Json;
using System.Text.Json.Nodes;

namespace V9MvcCoreProject.Extensions.LogsHelpers;

public class LogsParamEncryption
{// Create a lookup instead of dictionary (case-insensitive)
    private readonly ILookup<string, (int start, int end)> _maskingRules;

    public LogsParamEncryption()
    {
        var rules = new (string key, (int start, int end) rule)[]
        {
    // Passwords: show 1 char at start and end
    ("password", (0,1)),
    ("currentpassword", (1, 1)),
    ("confirmpassword", (1, 1)),
    ("newpassword", (1, 1)),
    ("reenterpassword", (1, 1)),

    // Card numbers: show 4 at start and 4 at end
    ("cardnumber", (4, 4)),
    ("creditcardnumber", (4, 4)),
    ("fromcardnumber", (4, 4)),
    ("tocardnumber", (4, 4)),
    ("encryptedcardnumber", (4, 4)),

    // OTP / PINs: show last 2 digits only
    ("otp", (0, 2)),
    ("pin", (0, 2)),
    ("apppin", (0, 2)),
    ("cardpin", (0, 2)),
    ("newpin", (0, 2)),
    ("confirmapppin", (0, 2)),
    ("currentpin", (0, 2)),
    ("confirmpin", (0, 2)),
    ("newcardpin", (0, 2)),
    ("newcardpinconfrim", (0, 2)),
    ("newpinconfirm", (0, 2)),

    // Default for other sensitive fields
    ("clientSecret", (2, 2)),
    ("encryptkey", (2, 2)),
    ("accesstoken", (3, 3)),
    ("base64", (3, 3)),
    ("token", (5,5))
        };

        // Build ILookup for fast grouping + case-insensitive search
        _maskingRules = rules
            .ToLookup(x => x.key, x => x.rule, StringComparer.OrdinalIgnoreCase);
    }

    public string CredentialsEncryptionResponse(string req)
    {
        try
        {
            if (req != "PAGE GET REQUEST" && !string.IsNullOrEmpty(req))
            {
                var obj = JsonNode.Parse(req);

                if (obj is JsonObject jsonObj)
                {
                    EncryptSensitiveDataResponse(jsonObj);
                    req = jsonObj.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
                }
            }
            return req;
        }
        catch (Exception)
        {
            return req;
        }
    }

    private void EncryptSensitiveDataResponse(JsonObject jsonObj)
    {
        foreach (var property in jsonObj.ToList())
        {
            if (_maskingRules.Contains(property.Key))
            {
                if (property.Value is not null)
                {
                    string originalValue = property.Value.ToString();
                    jsonObj[property.Key] = MaskValue(property.Key, originalValue);
                }
            }
            else if (property.Value is JsonObject nestedObject)
            {
                EncryptSensitiveDataResponse(nestedObject);
            }
            else if (property.Value is JsonArray jsonArray)
            {
                foreach (var item in jsonArray.OfType<JsonObject>())
                {
                    EncryptSensitiveDataResponse(item);
                }
            }
        }
    }

    public string CredentialsEncryptionRequest(string req)
    {
        try
        {
            if (req == "PAGE GET REQUEST" || string.IsNullOrWhiteSpace(req))
                return req;

            if (req.StartsWith("\"") && req.EndsWith("\""))
            {
                req = JsonSerializer.Deserialize<string>(req);
            }

            var jsonNode = JsonNode.Parse(req);

            if (jsonNode is JsonObject jsonObj)
            {
                EncryptSensitiveDataRequest(jsonObj);
                return jsonObj.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
            }

            return req;
        }
        catch
        {
            return req;
        }
    }

    private void EncryptSensitiveDataRequest(JsonObject jsonObj)
    {
        foreach (var property in jsonObj.ToList())
        {
            if (_maskingRules.Contains(property.Key))
            {
                if (property.Value is not null)
                {
                    string originalValue = property.Value.ToString();
                    jsonObj[property.Key] = MaskValue(property.Key, originalValue);
                }
            }
            else if (property.Value is JsonObject nestedObject)
            {
                EncryptSensitiveDataRequest(nestedObject);
            }
            else if (property.Value is JsonArray jsonArray)
            {
                foreach (var item in jsonArray.OfType<JsonObject>())
                {
                    EncryptSensitiveDataRequest(item);
                }
            }
        }
    }

    private string MaskValue(string key, string value)
    {
        if (string.IsNullOrEmpty(value)) return "****";

        // Get first matching rule (ILookup allows multiple but we just take first)
        var rule = _maskingRules[key].FirstOrDefault();
        var (visibleStart, visibleEnd) = rule == default ? (2, 2) : rule;

        if (value.Length <= visibleStart + visibleEnd)
            return new string('*', value.Length);

        string start = visibleStart > 0 ? value.Substring(0, visibleStart) : string.Empty;
        string end = visibleEnd > 0 ? value.Substring(value.Length - visibleEnd) : string.Empty;
        string middle = new string('*', value.Length - visibleStart - visibleEnd);

        return $"{start}{middle}{end}";
    }
}
