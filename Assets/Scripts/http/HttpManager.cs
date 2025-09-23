using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HTTPManager : MonoBehaviour
{
    [System.Serializable]
    public class ApiResponse
    {
        public string message;
        public int status;
    }

    // GET Request
    public IEnumerator GetRequest<T>(
        string url,
        System.Action<T> onSuccess,
        System.Action<string> onError
    )
    {
        // Debug.Log($"URL: {url}");
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            req.downloadHandler = new DownloadHandlerBuffer();

            req.SetRequestHeader("Content-Type", "application/json");

            // Debug.Log("Raw get response: " + req.downloadHandler.text);

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                // Debug.Log("GET REQ  SUCCESS");
                T response = JsonUtility.FromJson<T>(req.downloadHandler.text);
                // Debug.Log(response);

                if (response == null)
                {
                    onError.Invoke("Response success but recieved null value");
                }

                onSuccess.Invoke(response);
            }
            else
            {
                Debug.Log("Error in get request");
                onError.Invoke(req.error);
            }
        }
    }

    // POST Request
    public IEnumerator PostRequest<T>(
        string url,
        string jsonData,
        System.Action<T> onSuccess,
        System.Action<string> onError
    )
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            // Set headers
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer your-token-here");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                T postResponse = JsonUtility.FromJson<T>(request.downloadHandler.text);
                onSuccess?.Invoke(postResponse);
            }
            else
            {
                onError?.Invoke($"Error: {request.error}\nResponse Code: {request.responseCode}");
            }
        }
    }

    // PUT Request
    public IEnumerator PutRequest<T>(
        string url,
        string jsonData,
        System.Action<T> onSuccess,
        System.Action<string> onError
    )
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "PUT"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            // Set headers
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer your-token-here");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                T postResponse = JsonUtility.FromJson<T>(request.downloadHandler.text);
                onSuccess?.Invoke(postResponse);
            }
            else
            {
                onError?.Invoke($"Error: {request.error}\nResponse Code: {request.responseCode}");
            }
        }
    }

    // DELETE Request
    public IEnumerator DeleteRequest(
        string url,
        System.Action<string> onSuccess,
        System.Action<string> onError
    )
    {
        using (UnityWebRequest request = UnityWebRequest.Delete(url))
        {
            // Optional: Add headers
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer your-token-here");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(request.downloadHandler.text);
            }
            else
            {
                onError?.Invoke($"Error: {request.error}\nResponse Code: {request.responseCode}");
            }
        }
    }

    // Helper method to create JSON from objects
    public string ToJson<T>(T obj)
    {
        return JsonUtility.ToJson(obj);
    }

    // Helper method to parse JSON to objects
    public T FromJson<T>(string json)
    {
        return JsonUtility.FromJson<T>(json);
    }
}
