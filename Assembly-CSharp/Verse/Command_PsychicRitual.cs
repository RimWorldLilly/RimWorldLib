using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000606 RID: 1542
	public class Command_PsychicRitual : Command_Action
	{
		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x06003055 RID: 12373 RVA: 0x00135E8D File Offset: 0x0013408D
		public override Texture2D BGTexture
		{
			get
			{
				return ContentFinder<Texture2D>.Get("UI/PsychicRituals/PsychicRitualButtonBackground", true);
			}
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x06003056 RID: 12374 RVA: 0x00135E9A File Offset: 0x0013409A
		public override Texture2D BGTextureShrunk
		{
			get
			{
				return ContentFinder<Texture2D>.Get("UI/PsychicRituals/PsychicRitualButtonBackground", true);
			}
		}
	}
}
