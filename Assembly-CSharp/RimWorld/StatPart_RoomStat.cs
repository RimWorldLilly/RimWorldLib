using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FA4 RID: 8100
	public class StatPart_RoomStat : StatPart
	{
		// Token: 0x0600CDBF RID: 52671 RVA: 0x004B9A75 File Offset: 0x004B7C75
		public void PostLoad()
		{
			this.untranslatedCustomLabel = this.customLabel;
		}

		// Token: 0x0600CDC0 RID: 52672 RVA: 0x004B9A84 File Offset: 0x004B7C84
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Room room = req.Thing.GetRoom(RegionType.Set_All);
				if (room != null)
				{
					val *= room.GetStat(this.roomStat);
				}
			}
		}

		// Token: 0x0600CDC1 RID: 52673 RVA: 0x004B9AC0 File Offset: 0x004B7CC0
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Room room = req.Thing.GetRoom(RegionType.Set_All);
				if (room != null)
				{
					string str;
					if (!this.customLabel.NullOrEmpty())
					{
						str = this.customLabel;
					}
					else
					{
						str = this.roomStat.LabelCap;
					}
					return str + ": x" + room.GetStat(this.roomStat).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x04008332 RID: 33586
		private RoomStatDef roomStat;

		// Token: 0x04008333 RID: 33587
		[MustTranslate]
		private string customLabel;

		// Token: 0x04008334 RID: 33588
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedCustomLabel;
	}
}
