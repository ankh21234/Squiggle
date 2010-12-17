﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Squiggle.UI.Settings
{
    public class ContactGroups: ObservableCollection<ContactGroup>
    {
        public void Add(string groupName)
        {
            this.Add(new ContactGroup() 
            { 
                GroupName = groupName, 
                LastUsed = DateTime.UtcNow 
            });
        }

        protected override void SetItem(int index, ContactGroup item)
        {
            base.SetItem(index, item);

            ContactGroup existing = this.FirstOrDefault(x => x.Equals(item));
            if (existing == null)
                base.InsertItem(index, item);
            else
                existing.LastUsed = DateTime.UtcNow;
        }

        public void FlushItems()
        {
            FlushItems(TimeSpan.FromDays(7));
        }
        
        public void FlushItems(TimeSpan olderThan)
        {
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                TimeSpan life = DateTime.UtcNow.Subtract(Items[i].LastUsed);
                if (life >= olderThan)
                    this.RemoveAt(i);
            }
        }
    }
}
