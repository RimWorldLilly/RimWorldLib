using System;
using System.Runtime.CompilerServices;

namespace LudeonTK
{
	// Token: 0x0200005C RID: 92
	public struct ByteBits
	{
		// Token: 0x17000076 RID: 118
		public bool this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return ((int)this.intValue & 1 << index) != 0;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				if (value)
				{
					this.intValue = (byte)((int)this.intValue | 1 << index);
					return;
				}
				this.intValue = (byte)((int)this.intValue & ~(1 << index));
			}
		}

		// Token: 0x0400015E RID: 350
		private byte intValue;
	}
}
