using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000259 RID: 601
	public class DrawStyle_Line : DrawStyle
	{
		// Token: 0x06001196 RID: 4502 RVA: 0x00064BE0 File Offset: 0x00062DE0
		public override void Update(IntVec3 origin, IntVec3 target, List<IntVec3> buffer)
		{
			if (Mathf.Abs(origin.x - target.x) >= Mathf.Abs(origin.z - target.z))
			{
				int z = origin.z;
				if (origin.x > target.x)
				{
					IntVec3 intVec = target;
					target = origin;
					origin = intVec;
				}
				for (int i = origin.x; i <= target.x; i++)
				{
					buffer.Add(new IntVec3(i, 0, z));
				}
				return;
			}
			int x = origin.x;
			if (origin.z > target.z)
			{
				IntVec3 intVec2 = target;
				target = origin;
				origin = intVec2;
			}
			for (int j = origin.z; j <= target.z; j++)
			{
				buffer.Add(new IntVec3(x, 0, j));
			}
		}
	}
}
