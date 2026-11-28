using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001356 RID: 4950
	public class CompFlickable : ThingComp
	{
		// Token: 0x170014E4 RID: 5348
		// (get) Token: 0x06007A88 RID: 31368 RVA: 0x002BA6F7 File Offset: 0x002B88F7
		private CompProperties_Flickable Props
		{
			get
			{
				return (CompProperties_Flickable)this.props;
			}
		}

		// Token: 0x170014E5 RID: 5349
		// (get) Token: 0x06007A89 RID: 31369 RVA: 0x002BA704 File Offset: 0x002B8904
		private Texture2D CommandTex
		{
			get
			{
				if (this.cachedCommandTex == null)
				{
					this.cachedCommandTex = ContentFinder<Texture2D>.Get(this.Props.commandTexture, true);
				}
				return this.cachedCommandTex;
			}
		}

		// Token: 0x170014E6 RID: 5350
		// (get) Token: 0x06007A8A RID: 31370 RVA: 0x002BA731 File Offset: 0x002B8931
		// (set) Token: 0x06007A8B RID: 31371 RVA: 0x002BA73C File Offset: 0x002B893C
		public bool SwitchIsOn
		{
			get
			{
				return this.switchOnInt;
			}
			set
			{
				if (this.switchOnInt == value)
				{
					return;
				}
				this.switchOnInt = value;
				if (this.switchOnInt)
				{
					this.parent.BroadcastCompSignal("FlickedOn");
				}
				else
				{
					this.parent.BroadcastCompSignal("FlickedOff");
				}
				if (this.parent.Spawned)
				{
					this.parent.Map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlagDefOf.Buildings | MapMeshFlagDefOf.Things);
				}
			}
		}

		// Token: 0x170014E7 RID: 5351
		// (get) Token: 0x06007A8C RID: 31372 RVA: 0x002BA7C8 File Offset: 0x002B89C8
		public Graphic CurrentGraphic
		{
			get
			{
				if (this.SwitchIsOn)
				{
					return this.parent.DefaultGraphic;
				}
				if (this.offGraphic == null)
				{
					this.offGraphic = GraphicDatabase.Get(this.parent.def.graphicData.graphicClass, this.parent.def.graphicData.texPath + "_Off", this.parent.def.graphicData.shaderType.Shader, this.parent.def.graphicData.drawSize, this.parent.DrawColor, this.parent.DrawColorTwo, null);
				}
				return this.offGraphic;
			}
		}

		// Token: 0x06007A8D RID: 31373 RVA: 0x002BA87F File Offset: 0x002B8A7F
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.switchOnInt, "switchOn", true, false);
			Scribe_Values.Look<bool>(ref this.wantSwitchOn, "wantSwitchOn", true, false);
		}

		// Token: 0x06007A8E RID: 31374 RVA: 0x002BA8AB File Offset: 0x002B8AAB
		public bool WantsFlick()
		{
			return this.wantSwitchOn != this.switchOnInt;
		}

		// Token: 0x06007A8F RID: 31375 RVA: 0x002BA8BE File Offset: 0x002B8ABE
		public void DoFlick()
		{
			this.SwitchIsOn = !this.SwitchIsOn;
			SoundDefOf.FlickSwitch.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
		}

		// Token: 0x06007A90 RID: 31376 RVA: 0x002BA8FA File Offset: 0x002B8AFA
		public void ResetToOn()
		{
			this.switchOnInt = true;
			this.wantSwitchOn = true;
		}

		// Token: 0x06007A91 RID: 31377 RVA: 0x002BA90A File Offset: 0x002B8B0A
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompFlickable.<CompGetGizmosExtra>d__20 <CompGetGizmosExtra>d__ = new CompFlickable.<CompGetGizmosExtra>d__20(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x04004D68 RID: 19816
		private bool switchOnInt = true;

		// Token: 0x04004D69 RID: 19817
		private bool wantSwitchOn = true;

		// Token: 0x04004D6A RID: 19818
		private Graphic offGraphic;

		// Token: 0x04004D6B RID: 19819
		private Texture2D cachedCommandTex;

		// Token: 0x04004D6C RID: 19820
		private const string OffGraphicSuffix = "_Off";

		// Token: 0x04004D6D RID: 19821
		public const string FlickedOnSignal = "FlickedOn";

		// Token: 0x04004D6E RID: 19822
		public const string FlickedOffSignal = "FlickedOff";
	}
}
