using File = TagLib.File;

namespace YoutubePlaylistDownloader.Utilities;

public static class ExtensionMethods
{
    public static async Task WhenAll(params ValueTask[] tasks)
    {
        var toAwait = new List<Task>();

        foreach (var valueTask in tasks)
        {
            if (!valueTask.IsCompletedSuccessfully)
                toAwait.Add(valueTask.AsTask());
        }

        await Task.WhenAll(toAwait).ConfigureAwait(false);
    }

    public static async Task<bool> BulkFileExists(IVideo video, int vIndex, string file, string fileType, string savePath, FullPlaylist playlist = null)
    {
        var fullVideo = await GlobalConsts.YoutubeClient.Videos.GetAsync(video.Id).ConfigureAwait(false);
        string tagged = await GlobalConsts.TagMusicFile(fullVideo, file, vIndex, returnTitleOnly: true);
        string taggedBasedOnTitle = await GlobalConsts.TagFileBasedOnTitle(video, vIndex, file, playlist, true);

        var taggedPath = $"{savePath}\\{tagged}.{fileType}";
        var titlePath = $"{savePath}\\{taggedBasedOnTitle}.{fileType}";

        var f1 = System.IO.File.Exists(taggedPath);
        var f2 = System.IO.File.Exists(titlePath);
        return System.IO.File.Exists(taggedPath) || System.IO.File.Exists(titlePath);
    }
}