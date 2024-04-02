using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RNGroot
{

    /// <summary>
    /// An L-system alters a given string with a set of rewriting rules
    /// The string starts with an axiom symbol
    /// The string can be interpreted graphically to draw interesting structures
    /// 
    /// The string along with its rewriting rules will allow my algorithm to decide
    /// when to branch, when to produce buds, and generally how to place them.
    /// </summary>
    /// 
    public class LSystem : GrowthAlgorithm
    {
        
        private string l_system_string = "";
        private char axiom = 'A';
        private (string, string)[] rewriting_rules = { ("A", "NB"), ("B", "UNX"), ("X", "LBR"), ("L", "l(B)"), ("R", "r(B)") };
        protected override void Start()
        {
            l_system_string += axiom;
            base.Start();
        }
        public override void Grow()
        {
            string rewritten = l_system_string;
            for (int i = 0; i < rewritten.Length; i++)
            {
                // "i" is our current index in the string.
                // Now we loop through our rewriting rules and replace as necessary.
                foreach ((string A, string B) in rewriting_rules)
                {
                    int A_length = A.Length;

                    // Can't find A if the string isn't long enough to have it anymore.
                    if (i + A_length > rewritten.Length)
                        continue;

                    string compare_A = rewritten.Substring(i, A_length);
                    if (compare_A != A)
                        continue;

                    rewritten = rewritten.Remove(i, A_length);
                    rewritten = rewritten.Insert(i, B);
                    i += B.Length - 1;
                    break;
                }
            }
            l_system_string = rewritten;
            Debug.Log(l_system_string.ToString());
        }
    }
}