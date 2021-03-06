﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequentialTree
{
    public class Tree
    {
        static readonly int MaxDeep = 10;
        Node root;
        Formula formula;
        Queue<Node> leaves = new Queue<Node>();
        Queue<Node> unclosedLeaves = new Queue<Node>();
        List<Example> counterExamples;
        LogicalValue consequence = LogicalValue.Undetermined;
        public Node Root { get { return root; } }
        public List<Example> CounterExamples
        {
            get
            {
                if (counterExamples == null && consequence != LogicalValue.Undetermined)
                {
                    counterExamples = new List<Example>();
                    foreach (var leave in unclosedLeaves)
                    {
                        Example counterExample = leave.Value.CounterExample;
                        if (counterExample != null)
                            counterExamples.Add(counterExample);
                    }
                }
                return counterExamples;
            }
        }
        public Tree(Formula formula)
        {
            formula.Value = LogicalValue.False;
            root = new Node(new Sequence(formula));
            this.formula = formula;
        }
        public LogicalValue Check()
        {
            VarNamesGenerator.Reset();
            VarNamesGenerator.AddUsedNames(formula.FreeVarNames());
            update(this.root);
            int deep = 1;
            while (leaves.Count != 0 && deep <= MaxDeep)
            {
                activateNode(leaves.Dequeue());
                ++deep;
            }

            if (deep > MaxDeep) consequence = LogicalValue.Undetermined;
            else if (unclosedLeaves.Count == 0) consequence = LogicalValue.True;
            else consequence = LogicalValue.False;
            return consequence;
        }
        private void update(Node node)
        {
            if (!node.Value.IsClosed())
                if (node.Value.IsAtomic())
                    unclosedLeaves.Enqueue(node);
                else
                    leaves.Enqueue(node);
        }
        private void activateNode(Node node)
        {
            Sequence[] expanded = node.Value.Expand();
            Node child = new Node(expanded[0]);
            update(child);
            node.Childs.Add(child);
            if (expanded.Length > 1)
            {
                child = new Node(expanded[1]);
                update(child);
                node.Childs.Add(child);
            }
        }

    }
    public class Node
    {
        Sequence value;
        List<Node> childs = new List<Node>();
        public List<Node> Childs { get { return childs; } }
        public Sequence Value { get { return value; } set { this.value = value; } } 
        public Node(Sequence sequence)
        {
            this.value = sequence;
        }
        public override string ToString()
        {
            return value.ToString();
        }
    }
}
