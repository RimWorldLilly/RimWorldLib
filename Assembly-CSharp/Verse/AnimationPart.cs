using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000110 RID: 272
	public abstract class AnimationPart
	{
		// Token: 0x1700016C RID: 364
		// (get) Token: 0x0600095B RID: 2395
		protected abstract Type DefaultWorker { get; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600095C RID: 2396 RVA: 0x00035896 File Offset: 0x00033A96
		public Type WorkerType
		{
			get
			{
				return this.workerType ?? this.DefaultWorker;
			}
		}

		// Token: 0x04000469 RID: 1129
		public Vector2 pivot = new Vector2(0.5f, 0.5f);

		// Token: 0x0400046A RID: 1130
		public RotationMode rotationMode;

		// Token: 0x0400046B RID: 1131
		public Type workerType;
	}
}
