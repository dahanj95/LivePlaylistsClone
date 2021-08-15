using DotNetEnv;
using FluentScheduler;
using LivePlaylistsClone.Channels;
using LivePlaylistsClone.Services;
using System;

namespace LivePlaylistsClone
{
    class Program
    {
        static void Main(string[] args)
        {
            Env.Load();

            JobManager.Initialize(
                SpotifyOAuthService.Instance,
                new GlglzChannel(),
                new Kan88Channel()
            );

            Console.ReadKey();
        }
    }
}
