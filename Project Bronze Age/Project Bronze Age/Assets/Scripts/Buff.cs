using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable()]
public class Buff
{
    private string buffName;
    private string buffDescription;

    public List<BuffEffect> effects;
}