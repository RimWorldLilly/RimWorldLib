using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200236D RID: 9069
	public static class WorldObjectMaker
	{
		// Token: 0x0600E0F3 RID: 57587 RVA: 0x00538CC8 File Offset: 0x00536EC8
		public static WorldObject MakeWorldObject(WorldObjectDef def)
		{
			WorldObject obj = (WorldObject)Activator.CreateInstance(def.worldObjectClass);
			return WorldObjectMaker.MakeWorldObject(def, obj);
		}

		// Token: 0x0600E0F4 RID: 57588 RVA: 0x00538CED File Offset: 0x00536EED
		public static WorldObject MakeWorldObject(WorldObjectDef def, WorldObject obj)
		{
			obj.def = def;
			obj.ID = Find.UniqueIDsManager.GetNextWorldObjectID();
			obj.creationGameTicks = Find.TickManager.TicksGame;
			obj.PostMake();
			return obj;
		}
	}
}
