using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Security.Cryptography;
using SimpleJSON;
using System.Collections.Generic;

public static class AWSWebFormBuilder
{
    public static WWWForm BuildUploadRequest(JSONNode fields, Texture2D textureScreenshot, string fileName)
    {
        WWWForm form = new WWWForm();

        // Assuming fields["policy"] is already a base64-encoded string, so we directly use its value
        string policy = fields["policy"].Value;

        string encode = "png";
        byte[] image = ImageHandler.GetTextureBytes(textureScreenshot, encode); // Ensure this method returns the correct byte array for the image
        string date = fields["x-amz-date"].Value; // Use .Value to get the string without quotes

        // Use .Value for all string fields to ensure you don't include additional quotation marks
        form.AddField("key", fields["key"].Value);
        form.AddField("x-amz-algorithm", fields["x-amz-algorithm"].Value);
        form.AddField("x-amz-credential", fields["x-amz-credential"].Value);
        form.AddField("x-amz-date", date);
        form.AddField("x-amz-security-token", fields["x-amz-security-token"].Value);
        form.AddField("policy", policy); // No need to re-encode to base64
        form.AddField("x-amz-signature", fields["x-amz-signature"].Value);
        form.AddBinaryData("file", image, fileName, $"image/{encode}");

        return form;
    }
}