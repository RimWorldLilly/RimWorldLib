using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200060B RID: 1547
	public class Command_Toggle : Command
	{
		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x0600306D RID: 12397 RVA: 0x00136851 File Offset: 0x00134A51
		public override SoundDef CurActivateSound
		{
			get
			{
				if (this.isActive())
				{
					return this.turnOffSound;
				}
				return this.turnOnSound;
			}
		}

		// Token: 0x0600306E RID: 12398 RVA: 0x0013686D File Offset: 0x00134A6D
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.toggleAction();
		}

		// Token: 0x0600306F RID: 12399 RVA: 0x00136884 File Offset: 0x00134A84
		public override GizmoResult GizmoOnGUI(Vector2 loc, float maxWidth, GizmoRenderParms parms)
		{
			GizmoResult result = base.GizmoOnGUI(loc, maxWidth, parms);
			if (!this.disabled || !this.hideIconIfDisabled)
			{
				Rect rect = new Rect(loc.x, loc.y, this.GetWidth(maxWidth), 75f);
				Rect position = new Rect(rect.x + rect.width - 24f, rect.y, 24f, 24f);
				Texture2D image = this.isActive() ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex;
				GUI.DrawTexture(position, image);
			}
			return result;
		}

		// Token: 0x06003070 RID: 12400 RVA: 0x00136914 File Offset: 0x00134B14
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			Command_Toggle command_Toggle = other as Command_Toggle;
			return command_Toggle != null && command_Toggle.isActive() == this.isActive();
		}

		// Token: 0x04001F0B RID: 7947
		public Func<bool> isActive;

		// Token: 0x04001F0C RID: 7948
		public Action toggleAction;

		// Token: 0x04001F0D RID: 7949
		public SoundDef turnOnSound = SoundDefOf.Checkbox_TurnedOn;

		// Token: 0x04001F0E RID: 7950
		public SoundDef turnOffSound = SoundDefOf.Checkbox_TurnedOff;

		// Token: 0x04001F0F RID: 7951
		public bool activateIfAmbiguous = true;

		// Token: 0x04001F10 RID: 7952
		public bool hideIconIfDisabled;
	}
}
