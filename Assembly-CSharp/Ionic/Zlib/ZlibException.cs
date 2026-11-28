using System;
using System.Runtime.InteropServices;

namespace Ionic.Zlib
{
	// Token: 0x0200241F RID: 9247
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000E")]
	public class ZlibException : Exception
	{
		// Token: 0x0600E7E9 RID: 59369 RVA: 0x00565111 File Offset: 0x00563311
		public ZlibException()
		{
		}

		// Token: 0x0600E7EA RID: 59370 RVA: 0x00565119 File Offset: 0x00563319
		public ZlibException(string s) : base(s)
		{
		}
	}
}
