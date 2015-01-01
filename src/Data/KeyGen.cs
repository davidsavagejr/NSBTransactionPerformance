using System;
using System.Threading;
using RustFlakes;

namespace Data
{
    public class KeyGen
    {
        public static Func<ushort> WorkerId = () => (ushort)Thread.CurrentThread.ManagedThreadId;
        private static readonly SqlServerBigIntOxidation Oxidation = new SqlServerBigIntOxidation(WorkerId());

        public static long NewKey
        {
            get
            {

                return Oxidation.Oxidize();
            }
        }
    }
}