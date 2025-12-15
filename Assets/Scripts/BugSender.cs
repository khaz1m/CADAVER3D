using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq; // Required for Linq methods like string.Join

// IMPORTANT: This script requires the following setup in the Inspector:
// 1. Attach this script to a GameObject (e.g., your BugReport_Page or Manager).
// 2. Assign each problem button to call ToggleProblem(string) with its specific text.
// 3. Assign the "Send" button to call SendFeedback().

public class BugSender : MonoBehaviour
{
    // *** NEW DISCORD WEBHOOK URL FOR BUG REPORTS ***
    private const string DiscordWebhookUrl = "https://discord.com/api/webhooks/1439964875399893202/IhfA_iphG0ZnFwZcOzz6PVXtHOWY-Pzm_nDwUa1xoNNHFh067Mts1tItYnIRJARANs-d";
    
    // Stores the list of problems selected by the user.
    private readonly HashSet<string> selectedProblems = new HashSet<string>();

    /// <summary>
    /// Called by the UI problem buttons to toggle the selection of a specific issue.
    /// This should be wired up to all problem buttons in the Bug Report UI.
    /// </summary>
    /// <param name="problem">The description of the problem to toggle (e.g., "AR Model is unstable/shaking").</param>
    public void ToggleProblem(string problem)
    {
        // Check if the problem is already in the set
        if (selectedProblems.Contains(problem))
        {
            // If selected, deselect it (remove from the set)
            selectedProblems.Remove(problem);
            Debug.Log($"Problem Deselected: {problem}. Total problems: {selectedProblems.Count}");
        }
        else
        {
            // If not selected, select it (add to the set)
            selectedProblems.Add(problem);
            Debug.Log($"Problem Selected: {problem}. Total problems: {selectedProblems.Count}");
        }
        
        // Optional: Implement visual feedback here (e.g., change button color/state)
    }

    /// <summary>
    /// Called by the "Send" button to compile the report and send it to Discord.
    /// </summary>
    public void SendFeedback()
    {
        if (selectedProblems.Count == 0)
        {
            Debug.LogError("No problem has been selected. Please choose at least one issue before sending.");
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
        // 1. Format the list of problems into a Discord-friendly list
        string problemList = string.Join("\n- ", selectedProblems.OrderBy(p => p)); // Sorts and formats with list bullets

        // 2. Construct the full message
        string message = 
            $"🚨 **NEW BUG REPORT RECEIVED (Cadaver3D)** 🚨\n\n" +
            $"**User Report:** The user reported {selectedProblems.Count} issues.\n" +
            $"**Problems Selected:**\n- {problemList}\n\n" +
            $"*Submitted from device ID:* `{SystemInfo.deviceUniqueIdentifier}`";

        // 3. Create the JSON payload for Discord
        // Use a simple JSON string, escaping special characters as needed for a single string content.
        string jsonPayload = "{\"content\": \"" + message.Replace("\"", "\\\"").Replace("\n", "\\n") + "\", \"username\": \"Cadaver3D Bug Reporter\", \"avatar_url\": \"https://placehold.co/100x100/DC2626/ffffff?text=BUG\"}";
        
        // 4. Prepare the web request
        using (UnityWebRequest www = new UnityWebRequest(DiscordWebhookUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            
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
                Debug.Log($"Successfully sent bug report to Discord. Total problems: {selectedProblems.Count}");
                // Optional: Transition to a "Report Sent" message
            }
            else
            {
                Debug.LogWarning($"[Discord Webhook Warning]: Received non-success status code {www.responseCode}. Response: {www.downloadHandler.text}");
            }
        }
        
        // Clear the problems after successful (or attempted) sending
        selectedProblems.Clear();
    }
}