using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HLib.Item.History
{
    public class HistoryManager<HistoryItemType>
    {
        private LinkedList<HistoryItemType> history;

        public LinkedListNode<HistoryItemType> Current
        { get; set; }

        public LinkedListNode<HistoryItemType> Last
        { 
            get { return this.history.Last; }
            set { this.setLast(value); }
        }

        public HistoryManager()
        {
            this.history = new LinkedList<HistoryItemType>();
        }

        public void AddLast(HistoryItemType _historyItem)
        {
            this.history.AddLast(_historyItem);
            this.Current = this.history.Last;
        }

        public void AddFirst(HistoryItemType _historyItem)
        {
            this.history.AddFirst(_historyItem);
            this.Current = this.history.First;
        }

        public void AddAfter(LinkedListNode<HistoryItemType> node, HistoryItemType _historyItem)
        {
            this.history.AddAfter(node, _historyItem);
            this.Current = node.Next;
        }


        private void setLast(LinkedListNode<HistoryItemType> _node)
        {
            this.Current = this.history.Last;

            while (_node != this.Current)
            {
                this.history.RemoveLast();
                this.Current = this.history.Last;

            }
        }

        
    }
}
