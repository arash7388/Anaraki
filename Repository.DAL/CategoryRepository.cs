﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations.Model;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Repository.DAL
{
    using Repository.Entity.Domain;

    public class CategoryRepository :BaseRepository<Category>
    {
        public List<Category> GetAllMainCats()
        {
            var res = from c in MTOContext.Categories where c.ParentId == null orderby c.Name select c;

            return res.ToList();
        }

        public List<Category> GetAllByParentId(int parentId)
        {
            if(parentId!=0)
            {
                var res = from c in MTOContext.Categories where c.ParentId == parentId orderby c.Name select c;
                return res.ToList();
            }

            return GetAllMainCats();


        }
        public List<CategoryHelper> GetAllWithParents()
        {
            var result = from p in MTOContext.Categories
                         join i in MTOContext.Categories
                on p.Parent equals i into ps
                         from pp in ps.DefaultIfEmpty()
                select new CategoryHelper
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name,
                    InsertDateTime = p.InsertDateTime,
                    ParentName =  pp.Name,
                    Status = p.Status,
                    ParentId = p.ParentId,
                    UpdateDateTime = p.UpdateDateTime
                };

            return result.ToList();

        }

        public List<Category> GethierarchicalTree(int? parentId = null)
        {
            var allCats = new BaseRepository<Category>().GetAll();

            return allCats.Where(c => c.ParentId == parentId)
                            .Select(c => new Category()
                            {
                                Id = c.Id,
                                Code = c.Code,
                                Name = c.Name,
                                ParentId = c.ParentId,
                                Children = GetChildren(allCats.ToList(), c.Id)
                            })
                            .ToList();
        }

        public List<Category> GetChildren(List<Category> cats, int parentId)
        {
            return cats.Where(c => c.ParentId == parentId)
                    .Select(c => new Category
                    {
                        Id = c.Id,
                        Code = c.Code,
                        Name = c.Name,
                        ParentId = c.ParentId,
                        Children = GetChildren(cats, c.Id)
                    })
                    .ToList();
        }
        public List<Category> GetFirstSixElements()
        {
            return MTOContext.Categories.Take(6).ToList();
        }

        public List<Category> GetLastSixElements()
        {
            return MTOContext.Categories.OrderByDescending(a=>a.InsertDateTime).Take(6).ToList();
        }

        public List<KeyValuePair<int, string>> GetAllIdName()
        {
            var cats = from c in MTOContext.Categories
                select new 
                {
                    Key = c.Id,
                    Value = c.Name
                };

            var result = new List<KeyValuePair<int, string>>();

            foreach (var item in cats)
            {
                result.Add(new KeyValuePair<int, string>(item.Key,item.Value));
            }

            return result;
        }

        public List<Category> GetAllLeafCategories()
        {
            var result = new List<Category>();

            var rootNodes = MTOContext.Categories.Where(a => a.ParentId == null).ToList();

            foreach (Category category in rootNodes)
            {
                var leaves = MTOContext.GetAllLeafCategories(category.Id.ToSafeString());
                foreach (Category leaf in leaves)
                {
                    result.Add(leaf);
                }
            }

            return result;
        }
        
        public Category GetByCatPropId(int catPropId)
        {
            var catProp = new CategoryPropRepository().GetById(catPropId);
            if (catProp != null)
            {
                var result = MTOContext.Categories.SingleOrDefault(a => a.Id == catProp.CategoryId);
                return result;
            }

            return null;
        }

        public string GetFullName(int catId)
        {
            var allcats = GetAll();
            var cat = GetById(catId);
            var list = new List<string>();

            
            while (cat != null && cat.ParentId != null) 
            {
                list.Add(cat.Name);
                cat = allcats.SingleOrDefault(a => a.Id == cat.ParentId);
            }

            if (cat != null) list.Add(cat.Name);

            return string.Join(" -> ", list.AsEnumerable().Reverse());
        }
    }

    public class CategoryHelper : Category
    {
        public string ParentName { get; set; }

        
    }
}
