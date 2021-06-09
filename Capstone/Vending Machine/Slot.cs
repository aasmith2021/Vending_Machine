using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class Slot
    {
        public Slot(string slotId)
        {
            this.SlotId = slotId;
        }

        public Slot(string slotId, Item item, decimal price, int quantity = 5)
        {

            this.SlotId = slotId;
            this.Price = price;

            for (int i = 0; i < quantity; i++)
            {
                this.ItemStack.Push(item);
            }
        }

        public string SlotId { get; }

        public Stack<Item> ItemStack { get; set; } = new Stack<Item>();

        public decimal Price { get; set; }

        public int Quantity
        {
            get
            {
                return ItemStack.Count;
            }
        }

        public Item ItemInSlot
        {
            get
            {
                if (ItemStack.Count > 0)
                {
                    return ItemStack.Peek();
                }
                else
                {
                    Item soldOutItem = new UnknownItem("SOLD OUT", "");
                    return soldOutItem;
                }
            }
        }
    }
}
