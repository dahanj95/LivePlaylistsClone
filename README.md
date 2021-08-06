# LivePlaylistsClone
 This is an open-source clone in C# of the original work by [Yaniv Lerman](https://www.facebook.com/yaniv.lerman) called [LivePlaylists](https://www.facebook.com/LivePlaylists). This project is intended for educational purpose only of the inner working of such project, I was bored so decided to write my own version of this cute little app.

# How it works?
The app monitors the channel every 30 seconds, it saves an 8 second chunk (128KB in size) of the stream and uploads it to the [AudD.io API](https://docs.audd.io/#recognize) for recognition. If no song was recognized (due to a broadcast), then the method returns without doing nothing and waits for next execution.

# To-do
- Finished implementing the spotify playlist support.
- [Galgalatz - 100 Last Songs](https://open.spotify.com/playlist/5mLHWcR8C3ObKYdKxTyzyY?si=7bbc1536145c40f0)
- [Kan 88 (כאן 88)](https://open.spotify.com/playlist/3SpUq03whlfRMwEHMRulNy?si=a8a4e73a2de84977)

# Extending channels
Currently I implemented Galgalatz (גלגל"צ) and Kan 88 (כאן 88), but extending to more channels is quite easy. You create a new *class* with the channel name (and "Channel" suffix) in the "**Channels**" folder and inherit from `BaseChannel`. Then, you supply three properties. A url of the stream (`StreamUrl`), channel name (`ChannelName`) and the id of the spotify playlist (`PlaylistId`) initialized at ctor.

# Final words
Remember, this is a clone... it might not work exactly as the original work by Yaniv, but it has potential to continue development into a real-world app used by others. You may fork and modify the source, develop on your own and do what ever you want.

# Demo
![Additional Demo](https://github.com/dahanj95/LivePlaylistsClone/blob/main/LivePlaylistsClone/demo2.png)
![Demo](https://github.com/dahanj95/LivePlaylistsClone/blob/main/LivePlaylistsClone/demo.png)
