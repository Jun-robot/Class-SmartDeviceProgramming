/*------------------------------------------------------------*/
// <summary>GameCanvas for Unity</summary>
// <author>Seibe TAKAHASHI</author>
// <remarks>
// (c) 2015-2024 Smart Device Programming.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </remarks>
/*------------------------------------------------------------*/
#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace GameCanvas
{
    public static class GcAssert
    {
        public static bool IsThrowException
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        } = true;

#pragma warning disable CS8777
        public static void IsNotNull<T>([NotNull] T? obj) where T : class
        {
            if (obj is null) Assert($"{typeof(T).Name} is null");
        }
#pragma warning restore CS8777

        public static void IsNull<T>(T? obj) where T : class
        {
            if (obj != null) Assert($"{typeof(T).Name} is not null");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Assert(string message)
        {
            if (IsThrowException)
            {
                throw new AssertionException($"[{nameof(GcAssert)}] {message}\n");
            }
        }

        public sealed class AssertionException : System.Exception
        {
            public AssertionException(string message) : base(message) { }
        }
    }
}
