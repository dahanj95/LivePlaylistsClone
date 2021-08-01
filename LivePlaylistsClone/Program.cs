using FluentScheduler;
using LivePlaylistsClone.Channels;
using System;
using System.Collections.Generic;

namespace LivePlaylistsClone
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // support hebrew characters on the terminal

            List<BaseChannel> channels = new List<BaseChannel>
            {
                new GlglzChannel()
            };

            JobManager.Initialize(channels.ToArray());

            Console.ReadKey();
        }
    }
}
