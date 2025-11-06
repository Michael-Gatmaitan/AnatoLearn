using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public static class RemoteImageLoader
{
    public static IEnumerator LoadInto(
        VisualElement target,
        string imageUrl,
        Action<Texture2D> onLoaded = null,
        Action<string> onError = null
    )
    {
        if (target == null)
        {
            onError?.Invoke("Target VisualElement is null");
            yield break;
        }

        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            onError?.Invoke("Image URL is null or empty");
            yield break;
        }

        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return request.SendWebRequest();

#if UNITY_2020_2_OR_NEWER
            bool hasError = request.result != UnityWebRequest.Result.Success;
#else
            bool hasError = request.isNetworkError || request.isHttpError;
#endif

            if (hasError)
            {
                onError?.Invoke($"Failed to load image: {request.error}");
                yield break;
            }

            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            if (texture == null)
            {
                onError?.Invoke("Downloaded texture is null");
                yield break;
            }

            target.style.backgroundImage = new StyleBackground(texture);
            target.style.backgroundPositionX = new StyleBackgroundPosition(
                new BackgroundPosition(BackgroundPositionKeyword.Center)
            );
            target.style.backgroundPositionY = new StyleBackgroundPosition(
                new BackgroundPosition(BackgroundPositionKeyword.Center)
            );
            // target.style.backgroundRepeat = new StyleBackgroundRepeat();
            target.style.backgroundSize = new StyleBackgroundSize(
                new BackgroundSize(BackgroundSizeType.Cover)
            );

            onLoaded?.Invoke(texture);
        }
    }
}
