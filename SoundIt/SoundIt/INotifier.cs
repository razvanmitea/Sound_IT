using System;
using System.Threading.Tasks;

namespace SoundIt
{
    interface INotification
    {
        Task StartAsync ();

        void Stop ();
    }
}

