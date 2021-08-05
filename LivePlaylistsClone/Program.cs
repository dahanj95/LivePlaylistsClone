using FluentScheduler;
using LivePlaylistsClone.Channels;
using System;
using System.Collections.Generic;
using DotNetEnv;

namespace LivePlaylistsClone
{
    class Program
    {
        static void Main(string[] args)
        {
            List<BaseChannel> channels = new List<BaseChannel>
            {
                new GlglzChannel(),
                //new Kan88Channel()
            };

            Env.Load();

            JobManager.Initialize(channels.ToArray());

            Console.ReadKey();
        }
    }
}
