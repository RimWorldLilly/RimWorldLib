using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E53 RID: 7763
	public class Command_SelectStorage : Command_Action
	{
		// Token: 0x17002511 RID: 9489
		// (get) Token: 0x0600C979 RID: 51577 RVA: 0x004A3112 File Offset: 0x004A1312
		public override Texture2D BGTexture
		{
			get
			{
				return TexCommand.SelectShelf;
			}
		}

		// Token: 0x17002512 RID: 9490
		// (get) Token: 0x0600C97A RID: 51578 RVA: 0x004A3119 File Offset: 0x004A1319
		public override Texture2D BGTextureShrunk
		{
			get
			{
				return TexCommand.SelectShelf;
			}
		}
	}
}
