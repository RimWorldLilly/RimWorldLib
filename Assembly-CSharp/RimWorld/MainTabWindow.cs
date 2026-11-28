using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F9F RID: 3999
	public abstract class MainTabWindow : Window
	{
		// Token: 0x17001174 RID: 4468
		// (get) Token: 0x060065EE RID: 26094 RVA: 0x002441E5 File Offset: 0x002423E5
		public virtual Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(1010f, 684f);
			}
		}

		// Token: 0x17001175 RID: 4469
		// (get) Token: 0x060065EF RID: 26095 RVA: 0x002441F6 File Offset: 0x002423F6
		public virtual MainTabWindowAnchor Anchor
		{
			get
			{
				return MainTabWindowAnchor.Left;
			}
		}

		// Token: 0x17001176 RID: 4470
		// (get) Token: 0x060065F0 RID: 26096 RVA: 0x002441FC File Offset: 0x002423FC
		public override Vector2 InitialSize
		{
			get
			{
				Vector2 requestedTabSize = this.RequestedTabSize;
				if (requestedTabSize.y > (float)(UI.screenHeight - 35))
				{
					requestedTabSize.y = (float)(UI.screenHeight - 35);
				}
				if (requestedTabSize.x > (float)UI.screenWidth)
				{
					requestedTabSize.x = (float)UI.screenWidth;
				}
				return requestedTabSize;
			}
		}

		// Token: 0x060065F1 RID: 26097 RVA: 0x0024424D File Offset: 0x0024244D
		public MainTabWindow() : base(null)
		{
			this.layer = WindowLayer.GameUI;
			this.soundAppear = null;
			this.soundClose = SoundDefOf.TabClose;
			this.doCloseButton = false;
			this.doCloseX = false;
			this.preventCameraMotion = false;
		}

		// Token: 0x060065F2 RID: 26098 RVA: 0x00244284 File Offset: 0x00242484
		protected override void SetInitialSizeAndPosition()
		{
			base.SetInitialSizeAndPosition();
			if (this.Anchor == MainTabWindowAnchor.Left)
			{
				this.windowRect.x = 0f;
			}
			else
			{
				this.windowRect.x = (float)UI.screenWidth - this.windowRect.width;
			}
			this.windowRect.y = (float)(UI.screenHeight - 35) - this.windowRect.height;
		}

		// Token: 0x060065F3 RID: 26099 RVA: 0x002442EE File Offset: 0x002424EE
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.def.closesWorldView)
			{
				Find.World.renderer.wantedMode = WorldRenderMode.None;
			}
		}

		// Token: 0x04003D61 RID: 15713
		public MainButtonDef def;
	}
}
