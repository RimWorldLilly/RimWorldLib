using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000607 RID: 1543
	[StaticConstructorOnStartup]
	public class Command_SetBedOwnerType : Command
	{
		// Token: 0x06003058 RID: 12376 RVA: 0x00135EB0 File Offset: 0x001340B0
		public Command_SetBedOwnerType(Building_Bed bed)
		{
			this.bed = bed;
			switch (bed.ForOwnerType)
			{
			case BedOwnerType.Colonist:
				this.defaultLabel = "CommandBedSetForColonistsLabel".Translate();
				this.icon = Command_SetBedOwnerType.ForColonistsTex;
				break;
			case BedOwnerType.Prisoner:
				this.defaultLabel = "CommandBedSetForPrisonersLabel".Translate();
				this.icon = Command_SetBedOwnerType.ForPrisonersTex;
				break;
			case BedOwnerType.Slave:
				this.defaultLabel = "CommandBedSetForSlavesLabel".Translate();
				this.icon = Command_SetBedOwnerType.ForSlavesTex;
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unknown owner type selected for bed: ");
				defaultInterpolatedStringHandler.AppendFormatted<BedOwnerType>(bed.ForOwnerType);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				break;
			}
			}
			this.defaultDesc = "CommandBedSetForOwnerTypeDesc".Translate();
		}

		// Token: 0x06003059 RID: 12377 RVA: 0x00135F90 File Offset: 0x00134190
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption("CommandBedSetForColonistsLabel".Translate(), delegate()
			{
				this.bed.SetBedOwnerTypeByInterface(BedOwnerType.Colonist);
			}, Command_SetBedOwnerType.ForColonistsTex, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
			list.Add(new FloatMenuOption("CommandBedSetForPrisonersLabel".Translate(), delegate()
			{
				if (!Building_Bed.RoomCanBePrisonCell(this.bed.GetRoom(RegionType.Set_All)) && !this.bed.ForPrisoners)
				{
					Messages.Message("CommandBedSetForPrisonersFailOutdoors".Translate(), this.bed, MessageTypeDefOf.RejectInput, false);
					return;
				}
				this.bed.SetBedOwnerTypeByInterface(BedOwnerType.Prisoner);
			}, Command_SetBedOwnerType.ForPrisonersTex, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
			list.Add(new FloatMenuOption("CommandBedSetForSlavesLabel".Translate(), delegate()
			{
				this.bed.SetBedOwnerTypeByInterface(BedOwnerType.Slave);
			}, Command_SetBedOwnerType.ForSlavesTex, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x04001EFF RID: 7935
		private Building_Bed bed;

		// Token: 0x04001F00 RID: 7936
		private static readonly Texture2D ForColonistsTex = ContentFinder<Texture2D>.Get("UI/Commands/ForColonists", true);

		// Token: 0x04001F01 RID: 7937
		private static readonly Texture2D ForSlavesTex = ContentFinder<Texture2D>.Get("UI/Commands/ForSlaves", true);

		// Token: 0x04001F02 RID: 7938
		private static readonly Texture2D ForPrisonersTex = ContentFinder<Texture2D>.Get("UI/Commands/ForPrisoners", true);
	}
}
