# LivePlaylistsClone
 This is an open-source clone in C# of the original work by [Yaniv Lerman](https://www.facebook.com/yaniv.lerman) called [LivePlaylists](https://www.facebook.com/LivePlaylists). This project is intended for educational purpose only of the inner working of such project, I was bored so decided to write my own version of this cute little app.

# How it works?
The app monitors the channel every 30 seconds, it saves an 8 second chunk (128KB in size) of the stream and uploads it to the [AudD.io API](https://docs.audd.io/#recognize) for recognition. If no song was recognized (due to a broadcast), then the method returns without doing nothing and waits for next execution.

# To-do
The only task left is to implement the upload of the recognized songs to a playlist on Spotify or Apple Music (you choose). I will do this very soon, you may follow the development of the project to see changes committed on github.

# Extending channels
Currently I implemented Galgalatz (גלגל"צ) only, but extending to more channels is quite easy. You create a new *class* with the channel name in the "**Channels**" folder and inherit from `BaseChannel`. Then, you supply a url of the stream initialized at ctor. You must implement the logic of `Execute` method which does the saving of the chunk and uploading to the api.

# Final words
Remember, this is a clone... it might not work exactly as the original work by Yaniv, but it has potential to continue development into a real-world app used by others. You may fork and modify the source, develop on your own and do what ever you want.
