using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E52 RID: 7762
	public static class ContainingSelectionUtility
	{
		// Token: 0x0600C970 RID: 51568 RVA: 0x004A2CB8 File Offset: 0x004A0EB8
		public static Gizmo SelectCarriedThingGizmo(Thing container, Thing carriedThing)
		{
			if (container == null || carriedThing == null)
			{
				return null;
			}
			if (!ContainingSelectionUtility.CanSelect(carriedThing, container))
			{
				return null;
			}
			string label = ContainingSelectionUtility.GizmoLabel(container, carriedThing);
			string description = ContainingSelectionUtility.GizmoDescription(container, carriedThing);
			return ContainingSelectionUtility.CreateSelectGizmo(label, description, carriedThing, carriedThing);
		}

		// Token: 0x0600C971 RID: 51569 RVA: 0x004A2CF0 File Offset: 0x004A0EF0
		public static Gizmo SelectContainingThingGizmo(Thing containedThing)
		{
			if (containedThing.ParentHolder is Corpse)
			{
				return null;
			}
			Pawn_CarryTracker pawn_CarryTracker = containedThing.ParentHolder as Pawn_CarryTracker;
			if (pawn_CarryTracker != null)
			{
				return ContainingSelectionUtility.CreateSelectGizmo("CommandSelectCarryingPawn", "CommandSelectCarryingPawnDesc", pawn_CarryTracker.pawn, null);
			}
			Thing thing = containedThing.ParentHolder as Thing;
			if (thing != null)
			{
				return ContainingSelectionUtility.CreateSelectGizmo("CommandSelectContainerThing", "CommandSelectContainerThingDesc", thing, null);
			}
			ThingComp thingComp = containedThing.ParentHolder as ThingComp;
			if (thingComp != null)
			{
				return ContainingSelectionUtility.CreateSelectGizmo("CommandSelectContainerThing", "CommandSelectContainerThingDesc", thingComp.parent, null);
			}
			if (containedThing.Spawned && containedThing.def.category == ThingCategory.Item)
			{
				List<Thing> thingList = containedThing.Position.GetThingList(containedThing.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Building_Storage building_Storage = thingList[i] as Building_Storage;
					if (building_Storage != null)
					{
						return ContainingSelectionUtility.CreateSelectStorageGizmo("CommandSelectContainerThing".Translate(building_Storage), "CommandSelectContainerThingDesc".Translate(), building_Storage, building_Storage, true);
					}
				}
			}
			return null;
		}

		// Token: 0x0600C972 RID: 51570 RVA: 0x004A2DFA File Offset: 0x004A0FFA
		public static IEnumerable<Thing> SelectableContainedThings(Thing container)
		{
			ContainingSelectionUtility.<SelectableContainedThings>d__2 <SelectableContainedThings>d__ = new ContainingSelectionUtility.<SelectableContainedThings>d__2(-2);
			<SelectableContainedThings>d__.<>3__container = container;
			return <SelectableContainedThings>d__;
		}

		// Token: 0x0600C973 RID: 51571 RVA: 0x004A2E0A File Offset: 0x004A100A
		private static bool CanSelect(Thing carriedThing, Thing container)
		{
			if (!carriedThing.def.selectable)
			{
				return false;
			}
			if (carriedThing is Pawn)
			{
				return container.def.containedPawnsSelectable;
			}
			return container.def.containedItemsSelectable;
		}

		// Token: 0x0600C974 RID: 51572 RVA: 0x004A2E3C File Offset: 0x004A103C
		private static Gizmo CreateSelectGizmo(string label, string description, Thing thingToSelect, Thing iconThing = null)
		{
			if (!thingToSelect.def.selectable)
			{
				return null;
			}
			float num = 1f;
			float iconAngle = 0f;
			Vector2 one = Vector2.one;
			Color white = Color.white;
			Pawn pawn = iconThing as Pawn;
			Texture icon;
			if (pawn != null && pawn.def.uiIcon == null)
			{
				Pawn pawn2 = pawn;
				Vector2 size = new Vector2(75f, 75f);
				Rot4 south = Rot4.South;
				PawnHealthState? healthStateOverride = new PawnHealthState?(PawnHealthState.Mobile);
				icon = PortraitsCache.Get(pawn2, size, south, default(Vector3), 1f, true, true, true, true, null, null, false, healthStateOverride);
			}
			else if (iconThing != null)
			{
				Material material;
				icon = Widgets.GetIconFor(iconThing, new Vector2(75f, 75f), new Rot4?(iconThing.def.defaultPlacingRot), true, out num, out iconAngle, out one, out white, out material);
			}
			else
			{
				icon = ((thingToSelect is Pawn) ? TexCommand.SelectCarriedPawn : TexCommand.SelectCarriedThing);
			}
			return new Command_SelectStorage
			{
				defaultLabel = label.Translate(thingToSelect).TruncateHeight(75f, 37.5f, ContainingSelectionUtility.labelCache),
				defaultDesc = description.Translate(),
				icon = icon,
				iconDrawScale = num * 0.85f,
				iconAngle = iconAngle,
				defaultIconColor = white,
				iconProportions = one,
				action = delegate()
				{
					Find.Selector.ClearSelection();
					Find.Selector.Select(thingToSelect, true, true);
				}
			};
		}

		// Token: 0x0600C975 RID: 51573 RVA: 0x004A2FC0 File Offset: 0x004A11C0
		public static Gizmo CreateSelectStorageGizmo(string label, string description, Thing thingToSelect, Thing iconThing = null, bool groupable = true)
		{
			if (!thingToSelect.def.selectable)
			{
				return null;
			}
			float num;
			float iconAngle;
			Vector2 iconProportions;
			Color defaultIconColor;
			Material material;
			Texture iconFor = Widgets.GetIconFor(iconThing, new Vector2(75f, 75f), new Rot4?(iconThing.def.defaultPlacingRot), false, out num, out iconAngle, out iconProportions, out defaultIconColor, out material);
			Text.Font = GameFont.Tiny;
			Command_SelectStorage command_SelectStorage = new Command_SelectStorage();
			command_SelectStorage.defaultLabel = label.TruncateHeight(75f, 37.5f, null);
			command_SelectStorage.defaultDesc = description;
			command_SelectStorage.icon = iconFor;
			command_SelectStorage.iconDrawScale = num * 0.85f;
			command_SelectStorage.iconAngle = iconAngle;
			command_SelectStorage.defaultIconColor = defaultIconColor;
			command_SelectStorage.iconProportions = iconProportions;
			command_SelectStorage.groupable = groupable;
			command_SelectStorage.groupKey = thingToSelect.thingIDNumber;
			command_SelectStorage.action = delegate()
			{
				Find.Selector.ClearSelection();
				Find.Selector.Select(thingToSelect, true, true);
			};
			Text.Font = GameFont.Small;
			return command_SelectStorage;
		}

		// Token: 0x0600C976 RID: 51574 RVA: 0x004A30A4 File Offset: 0x004A12A4
		private static string GizmoLabel(Thing container, Thing carriedThing)
		{
			if (container is Pawn)
			{
				if (carriedThing is Pawn)
				{
					return "CommandSelectCarriedPawn";
				}
				return "CommandSelectCarriedThing";
			}
			else
			{
				if (carriedThing is Pawn)
				{
					return "CommandSelectContainedPawn";
				}
				return "CommandSelectContainedThing";
			}
		}

		// Token: 0x0600C977 RID: 51575 RVA: 0x004A30D5 File Offset: 0x004A12D5
		private static string GizmoDescription(Thing container, Thing carriedThing)
		{
			if (container is Pawn)
			{
				if (carriedThing is Pawn)
				{
					return "CommandSelectCarriedPawnDesc";
				}
				return "CommandSelectCarriedThingDesc";
			}
			else
			{
				if (carriedThing is Pawn)
				{
					return "CommandSelectContainedPawnDesc";
				}
				return "CommandSelectContainedThingDesc";
			}
		}

		// Token: 0x040072B2 RID: 29362
		private static readonly Dictionary<string, TaggedString> labelCache = new Dictionary<string, TaggedString>();
	}
}
