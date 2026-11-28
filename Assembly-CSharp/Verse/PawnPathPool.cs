using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse.AI;

namespace Verse
{
	// Token: 0x020000EB RID: 235
	public class PawnPathPool
	{
		// Token: 0x06000882 RID: 2178 RVA: 0x00030767 File Offset: 0x0002E967
		public PawnPathPool(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00030784 File Offset: 0x0002E984
		public PawnPath GetPath()
		{
			PawnPath pawnPath;
			if (!this.paths.TryPop(out pawnPath))
			{
				pawnPath = new PawnPath();
				pawnPath.pool = this;
				int num = this.map.mapPawns.AllPawnsSpawnedCount * 2 + 1 + 4;
				int num2 = this.pathsCreated + 1;
				this.pathsCreated = num2;
				if (num2 > num)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(112, 4);
					defaultInterpolatedStringHandler.AppendLiteral("Leak suspected in object pool for ");
					defaultInterpolatedStringHandler.AppendFormatted("PawnPath");
					defaultInterpolatedStringHandler.AppendLiteral("s, created: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.pathsCreated);
					defaultInterpolatedStringHandler.AppendLiteral(", expected less than ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(num);
					defaultInterpolatedStringHandler.AppendLiteral(". Dispose them so they can be reused by the ");
					defaultInterpolatedStringHandler.AppendFormatted("PawnPathPool");
					defaultInterpolatedStringHandler.AppendLiteral(".");
					Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), 664788);
				}
			}
			return pawnPath;
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x00030863 File Offset: 0x0002EA63
		public void ReturnPath(PawnPath path)
		{
			this.paths.Push(path);
		}

		// Token: 0x040003CB RID: 971
		private readonly Map map;

		// Token: 0x040003CC RID: 972
		private int pathsCreated;

		// Token: 0x040003CD RID: 973
		private readonly Stack<PawnPath> paths = new Stack<PawnPath>(64);
	}
}
