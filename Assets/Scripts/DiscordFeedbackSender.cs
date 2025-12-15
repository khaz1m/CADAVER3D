using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

// IMPORTANT: This script requires the following setup in the Inspector:
// 1. Attach this script to a GameObject (e.g., your UserSatisfactory_Page).
// 2. Assign the 1-5 buttons to call SetRating(1), SetRating(2), etc.
// 3. Assign the "Send" button to call SendFeedback().

public class DiscordFeedbackSender : MonoBehaviour
{
    // The webhook URL provided by the user. Make sure it's correct.
    private const string DiscordWebhookUrl = "https://discord.com/api/webhooks/1439965804815712306/auoTNsFn8wOk2v5vuAEKSSTkZ528gLmU1QwkeiDyNxJBwOHWPvUgGX13r1fadktdawBu";
    
    // Stores the rating selected by the user (1-5)
    private int currentRating = 0;

    /// <summary>
    /// Called by the UI buttons (1, 2, 3, 4, 5) to set the current rating.
    /// </summary>
    /// <param name="rating">The numerical rating selected (1 to 5).</param>
    public void SetRating(int rating)
    {
        if (rating >= 1 && rating <= 5)
        {
            currentRating = rating;
            Debug.Log($"Feedback rating set to: {currentRating}");
            
            // Optional: You could visually update the UI here to show which button is selected.
        }
        else
        {
            Debug.LogError($"Invalid rating value: {rating}. Must be between 1 and 5.");
        }
    }

    /// <summary>
    /// Called by the "Send" button to compile the feedback and send it to Discord.
    /// </summary>
    public void SendFeedback()
    {
        if (currentRating == 0)
        {
            Debug.LogError("No rating has been selected. Please choose a score from 1 to 5 before sending.");
            // Optional: Show an in-game alert to the user.
            return;
        }

        // Start the asynchronous web request
        StartCoroutine(SendToDiscordCoroutine());
    }

    /// <summary>
    /// Coroutine to handle the asynchronous network request using UnityWebRequest.
    /// </summary>
    private IEnumerator SendToDiscordCoroutine()
    {
        // 1. Determine the descriptive level
        string feedbackLevel = GetFeedbackLevel(currentRating);

        // 2. Construct the full message
        string message = 
            $"📢 **New Cadaver3D Report!**\n" +
            $"**Score:** `{currentRating}`\n" +
            $"**Level:** {feedbackLevel}\n" + 
            $"*Submitted from user ID: {SystemInfo.deviceUniqueIdentifier}*"; // Example of adding system info

        // 3. Create the JSON payload for Discord
        // Use a simple JSON string since Unity's built-in JSON utility can be cumbersome for simple payloads
        string jsonPayload = "{\"content\": \"" + message.Replace("\"", "\\\"").Replace("\n", "\\n") + "\", \"username\": \"Cadaver3D Feedback Bot\", \"avatar_url\": \"https://placehold.co/100x100/7C3AED/ffffff?text=C3D\"}";
        
        // 4. Prepare the web request
        using (UnityWebRequest www = new UnityWebRequest(DiscordWebhookUrl, "POST"))
        {
            // Set up the body of the request
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            
            // Set the necessary content type header for Discord
            www.SetRequestHeader("Content-Type", "application/json");

            // 5. Send and wait for the response
            yield return www.SendWebRequest();

            // 6. Check for errors
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"[Discord Webhook Error]: {www.error}. Response Code: {www.responseCode}. Response Text: {www.downloadHandler.text}");
                // Optional: Notify the user in-game about the failure
            }
            else if (www.responseCode >= 200 && www.responseCode < 300)
            {
                Debug.Log($"Successfully sent feedback to Discord. Rating: {currentRating}");
                // Optional: Transition to a "Thank You" screen
            }
            else
            {
                // Non-2xx success, or unknown error
                Debug.LogWarning($"[Discord Webhook Warning]: Received non-success status code {www.responseCode}. Response: {www.downloadHandler.text}");
            }
        }
        
        // Reset the rating after sending
        currentRating = 0;
    }

    /// <summary>
    /// Translates a numerical rating into a descriptive string.
    /// </summary>
    private string GetFeedbackLevel(int rating)
    {
        return rating switch
        {
            5 => "(Very Satisfied)",
            4 => "(Buttons wont work as intended)",
            3 => " (Anatomy label is wrong)",
            2 => "(Model looks broken)",
            1 => "(Model are unstable)",
            _ => "(Error: Invalid Rating)",
        };
    }
}