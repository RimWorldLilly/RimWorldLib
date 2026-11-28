using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003BC RID: 956
	public class PawnRenderNodeProperties_NociosphereSegment : PawnRenderNodeProperties
	{
		// Token: 0x06001C8F RID: 7311 RVA: 0x000AFEA0 File Offset: 0x000AE0A0
		public PawnRenderNodeProperties_NociosphereSegment()
		{
			this.nodeClass = typeof(PawnRenderNode_NociosphereSegment);
			this.workerClass = typeof(PawnRenderNodeWorker_NociosphereSegment);
			this.drawSize = new Vector2(1.35f, 1.35f);
		}

		// Token: 0x04001758 RID: 5976
		[NoTranslate]
		public string maskPath;

		// Token: 0x04001759 RID: 5977
		public Vector2 offset;
	}
}
