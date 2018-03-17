
using System;
#if NETFX_CORE
using System.Threading.Tasks;
#endif
using System.Threading;

namespace Svelto.Utilities
{
    public static class ThreadUtility
    {
        public static void MemoryBarrier()
        {
#if NETFX_CORE || NET_4_6
            Interlocked.MemoryBarrier();
#else
            Thread.MemoryBarrier();
#endif
        }
#if NETFX_CORE && !NET_STANDARD_2_0 && !NETSTANDARD2_0
        static TimeSpan wait = TimeSpan.FromMilliseconds(0.1);
#endif
        public static void Yield()
        {
#if NETFX_CORE && !NET_STANDARD_2_0 && !NETSTANDARD2_0
            Task.Delay(wait).Wait();
#elif NET_4_6 || NET_STANDARD_2_0 || NETSTANDARD2_0
            Thread.Yield(); 
#else
            Thread.Sleep(0); 
#endif
        }

        public static void TakeItEasy()
        {
#if NETFX_CORE && !NET_STANDARD_2_0 && !NETSTANDARD2_0
            Task.Delay(1).Wait();
#elif NET_4_6 || NET_STANDARD_2_0 || NETSTANDARD2_0
            Thread.Sleep(1); 
#endif
        }
    }

#if NETFX_CORE || NET_4_6
    public sealed class ManualResetEventEx : ManualResetEventSlim
    {
        public new void Wait()
        {
            base.Wait();
        }

        public new void Wait(int ms)
        {
            base.Wait(ms);
        }

        public new void Reset()
        {
            base.Reset();
        }

        public new void Set()
        {
            base.Set();
        }

        public new void Dispose()
        {
            base.Dispose();
        }
    }
#else
    public class ManualResetEventEx
    {
        ManualResetEvent _manualReset = new ManualResetEvent(false);
        
        public void Wait()
        {
            _manualReset.WaitOne();
        }

        public void Wait(int ms)
        {
            _manualReset.WaitOne(ms);
        }

        public void Reset()
        {
            _manualReset.Reset();
        }

        public void Set()
        {
            _manualReset.Set();
        }

        public void Dispose()
        {
            _manualReset.Close();
        }
    }
#endif
}