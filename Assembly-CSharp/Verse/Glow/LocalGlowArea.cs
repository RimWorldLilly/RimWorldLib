using System;
using LudeonTK;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Verse.Glow
{
	// Token: 0x020007B9 RID: 1977
	public struct LocalGlowArea : IDisposable
	{
		// Token: 0x06003F9A RID: 16282 RVA: 0x00197594 File Offset: 0x00195794
		public static LocalGlowArea AllocateNew()
		{
			LocalGlowArea result = new LocalGlowArea
			{
				inUse = false,
				colors = new UnsafeList<Color32>(6561, Allocator.Persistent, NativeArrayOptions.ClearMemory)
			};
			result.colors.Resize(6561, NativeArrayOptions.ClearMemory);
			return result;
		}

		// Token: 0x06003F9B RID: 16283 RVA: 0x001975DE File Offset: 0x001957DE
		public void Take()
		{
			this.inUse = true;
		}

		// Token: 0x06003F9C RID: 16284 RVA: 0x001975E7 File Offset: 0x001957E7
		public void Return()
		{
			NativeArrayUtility.MemClear<Color32>(this.colors);
			this.inUse = false;
		}

		// Token: 0x06003F9D RID: 16285 RVA: 0x001975FB File Offset: 0x001957FB
		public void Dispose()
		{
			ref this.colors.EnsureDisposed<Color32>();
		}

		// Token: 0x04002A24 RID: 10788
		public bool inUse;

		// Token: 0x04002A25 RID: 10789
		public UnsafeList<Color32> colors;
	}
}
