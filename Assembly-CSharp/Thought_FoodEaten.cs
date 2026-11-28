using System;
using RimWorld;
using Verse;

// Token: 0x02000018 RID: 24
public class Thought_FoodEaten : Thought_Memory
{
	// Token: 0x17000023 RID: 35
	// (get) Token: 0x060000C9 RID: 201 RVA: 0x00008123 File Offset: 0x00006323
	public override string Description
	{
		get
		{
			return base.Description + "\n\n" + this.foodThoughtDescription;
		}
	}

	// Token: 0x060000CA RID: 202 RVA: 0x0000813C File Offset: 0x0000633C
	public void SetFood(Thing food)
	{
		CompIngredients compIngredients = food.TryGetComp<CompIngredients>();
		this.foodThoughtDescription = "ThoughtFoodEatenFood".Translate() + ": " + food.def.LabelCap;
		if (compIngredients != null && compIngredients.ingredients.Count > 0)
		{
			bool flag;
			this.foodThoughtDescription += " (" + "ThoughtFoodEatenIngredients".Translate() + ": " + compIngredients.GetIngredientsString(false, out flag) + ")";
		}
	}

	// Token: 0x060000CB RID: 203 RVA: 0x000081DC File Offset: 0x000063DC
	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look<string>(ref this.foodThoughtDescription, "foodThoughtDescription", null, false);
	}

	// Token: 0x04000031 RID: 49
	private string foodThoughtDescription;
}
