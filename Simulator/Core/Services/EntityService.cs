﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedLibrary.Enums;
using SharedLibrary.Interfaces.Entity;
using SharedLibrary.Interfaces.Service;

namespace Core.Services
{
	public class EntityService<TEntity> : BaseMethods, IEntityService<TEntity> where TEntity : class, IEntity<TEntity>
	{
		public bool AreSiblings(TEntity one, TEntity two)
		{
			this.CheckEntity(one, false);
			this.CheckEntity(two, false);
			return one.Siblings.Contains(two);
		}

		public bool AreConnected(TEntity current, TEntity ancestor)
		{
			this.CheckEntity(current, false);
			this.CheckEntity(ancestor, false);

			if (this.AreSiblings(current, ancestor)) {
				return true;
			}
			// TODO: What if ancestor is in another branch
			// 0<3 - Ancestor is younger than current Entity.
			// 3<0 - Ancestor is older than current Entity.
			if (current.Generation < ancestor.Generation) {
				return this.AreConnected(ancestor, current);
			}

			return current.Ancestors.Any(x => x.Id == ancestor.Id || ancestor.Ancestors.Any(y => x.Id == y.Id));
		}

		public IEnumerable<TEntity> GetLowestCommonAncestors(TEntity one, TEntity two)
		{
			if (this.AreSiblings(one, two)) {
				return new[] {one.Father, one.Mother};
			}

			var common = one.Ancestors
							.Where(x => two.Ancestors
										   .Any(y => y.Id == x.Id)
								  );

			// ReSharper disable once PossibleMultipleEnumeration
			var lastCommonGeneration = common.Select(x => x.Generation)
											 .Max();

			// ReSharper disable once PossibleMultipleEnumeration
			var lca = common.Where(x => x.Generation == lastCommonGeneration);

			return lca;
		}

		public double GetConnectionRate(TEntity current, TEntity ancestor)
		{
			this.CheckEntity(current, false);
			this.CheckEntity(ancestor, false);

			throw new NotImplementedException();
		}

		public double GetDegradation(TEntity mother, TEntity father)
		{
			this.CheckEntity(mother, false);
			this.CheckEntity(father, false);

			var parentDegeneration = mother.Degeneration + father.Degeneration;
			if (this.AreSiblings(mother, father)) {
				parentDegeneration += 0.5;
			}
			return parentDegeneration / 2;
		}

		public TEntity GetFirstAncestor(TEntity current, Genders gender)
		{
			this.CheckEntity(current, false);

			if (gender == Genders.Female) {
				if (current.Mother == null) {
					return current;
				}
				return this.GetFirstAncestor(current.Mother, gender);
			}
			if (current.Father == null) {
				return current;
			}
			return this.GetFirstAncestor(current.Father, gender);
		}
	}
}