using System;
using System.Threading.Tasks;

namespace SoundIt
{
    interface INotification
    {
        void StartAsync ();

        void Stop ();
    }
}

