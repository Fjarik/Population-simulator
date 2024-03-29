﻿using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using SharedLibrary;
using SharedLibrary.Interfaces.Entity;
using SharedLibrary.Enums;

namespace Core.Models
{
	/// <summary>
	/// Human or any living entity
	/// </summary>
	public sealed class Entity : IEntity<Entity>
	{
		/// <summary>
		/// Gets a value indicating whether this instance is alive.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is alive; otherwise, <c>false</c>.
		/// </value>
		public bool IsAlive => this.DeathCycle == null;

		/// <summary>
		/// Gets a value indicating whether Entity is single (does NOT have partner).
		/// </summary>
		/// <value>
		///   <c>true</c> if this Entity is single; otherwise, <c>false</c>.
		/// </value>
		public bool IsSingle => this.Partner == null;

		/// <summary>
		/// Gets a value indicating whether Entity has any child.
		/// </summary>
		/// <value>
		///   <c>true</c> if Entity has children; otherwise, <c>false</c>.
		/// </value>
		public bool HasChildren => this.Children.Any();

		/// <summary>
		/// Gets a value indicating whenther Entity is able to make child
		/// </summary>
		/// <value>
		///   <c>true</c> if Entity is able to make children; otherwise, <c>false</c>.
		/// </value>
		public bool IsAbleToMakeChildren => !this.IsSingle &&
											this.Partner.IsAlive &&
											this.Age.ChildrenMakeableAge(this.Gender);

		/// <summary>
		/// Gets or sets the unique identifier of the Entity.
		/// </summary>
		/// <value>
		/// The unique identifier.
		/// </value>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the gender of the Entity.
		/// </summary>
		/// <value>
		/// The gender.
		/// </value>
		public Genders Gender { get; set; }

		/// <summary>
		/// Gets or sets the age that was last cycle.
		/// </summary>
		/// <value>
		/// The last age.
		/// </value>
		public Ages? LastAge { get; set; }

		/// <summary>
		/// Gets or sets the age of the Entity.
		/// </summary>
		/// <value>
		/// The age.
		/// </value>
		public Ages Age { get; set; }

		/// <summary>
		/// Gets or sets the cycle when the Entity was born.
		/// </summary>
		/// <value>
		/// The born cycle.
		/// </value>
		public int BornCycle { get; set; }

		/// <summary>
		/// Gets or sets the cycle when the Entity died.
		/// </summary>
		/// <value>
		/// The death cycle.
		/// </value>
		public int? DeathCycle { get; set; } = null;

		/// <summary>
		/// Gets or sets the generation of the Entity.
		/// </summary>
		/// <value>
		/// The generation.
		/// </value>
		public int Generation { get; set; }

		/// <summary>
		/// Gets or sets the degeneration. (Whenever any of the ancestors had incest)
		/// (0-1)
		/// 0 - No degeneration, 1 - Completely degenerated (soon death)
		/// </summary>
		/// <value>
		/// The degeneration value.
		/// </value>
		public double Degeneration { get; set; }

#region Modifiers

		/// <summary>
		/// Gets or sets the attractiveness. (Change to find a partner)
		/// Larger number means bigger chance to find a partner.
		/// (0-1) Default value is 1.
		/// </summary>
		/// <value>
		/// The attractiveness.
		/// </value>
		public double Attractiveness { get; set; } = 1;

		/// <summary>
		/// Gets or sets the pontency modifier. (0-1)
		/// Larger number means bigger chance for more kids.
		/// Default value is 0.5.
		/// </summary>
		/// <value>
		/// The pontency modifier.
		/// </value>
		public double Pontency { get; set; } = 0.5;

		/// <summary>
		/// Gets or sets the modifier of long age. (0-1)
		/// Larger number means longer life.
		/// Default value is 0.5.
		/// </summary>
		/// <value>
		/// The longevity.
		/// </value>
		public double Longevity { get; set; } = 0.5;

#endregion

		/// <summary>
		/// Gets or sets the mother of the Entity.
		/// </summary>
		/// <value>
		/// The mother.
		/// </value>
		public Entity Mother { get; set; }

		/// <summary>
		/// Gets or sets the father of the Entity.
		/// </summary>
		/// <value>
		/// The father.
		/// </value>
		public Entity Father { get; set; }

		/// <summary>
		/// Gets the siblings of the Entity.
		/// </summary>
		/// <value>
		/// The siblings.
		/// </value>
		public List<Entity> Siblings =>
			this.Mother?.Children?.Where(x => x.Id != this.Id).ToList() ?? new List<Entity>();

		/// <summary>
		/// Gets or sets the partner of the Entity. (Wife or Husband)
		/// </summary>
		/// <value>
		/// The partner.
		/// </value>
		public Entity Partner { get; set; }

		/// <summary>
		/// Gets or sets the children of the Entity.
		/// </summary>
		/// <value>
		/// The children.
		/// </value>
		public List<Entity> Children { get; set; }

		public List<Entity> Ancestors { get; private set; }

		public Entity()
		{
			//this.Siblings = new List<Entity>();
			this.Children = new List<Entity>();
			this.Ancestors = new List<Entity>();
		}

		public void SetAncestors()
		{
			this.Ancestors.Add(this.Mother);
			this.Ancestors.Add(this.Father);
			this.AddAncestors(this.Mother.Ancestors);
			this.AddAncestors(this.Father.Ancestors);

			this.Ancestors = this.Ancestors.DistinctBy(x => x.Id).ToList();
		}

		private void AddAncestors(IEnumerable<Entity> motherAncestors)
		{
			this.Ancestors.AddRange(motherAncestors.Where(x => this.Ancestors.Any(y => x.Id != y.Id)));
		}
	}
}