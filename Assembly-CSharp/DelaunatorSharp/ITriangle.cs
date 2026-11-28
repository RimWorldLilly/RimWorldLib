using System;
using System.Collections.Generic;
using UnityEngine;

namespace DelaunatorSharp
{
	// Token: 0x0200242D RID: 9261
	public interface ITriangle
	{
		// Token: 0x17002931 RID: 10545
		// (get) Token: 0x0600E8A6 RID: 59558
		IEnumerable<Vector2> Points { get; }

		// Token: 0x17002932 RID: 10546
		// (get) Token: 0x0600E8A7 RID: 59559
		int Index { get; }
	}
}
