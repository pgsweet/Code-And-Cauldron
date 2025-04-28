# Commands:

#### <u>mov \<arg1> \<arg2> \<arg3></u>

\<arg1>: Must be a non-empty container
\<arg2>: Must be a container
\<arg3>: **Optional**; Determines the number of items to move, by default its all items

Allows the player to move \<arg3> items from \<arg1> to \<arg2>

**NOTE:** this can be used to combine items into one stack as long as they are the same type

---

#### <u>bot \<arg1> \<arg2></u>

\<arg1>: Must be an empty container
\<arg2>: **Optional**; Determines the number of potions to bottle, by default it is 1 potion

Bottles \<arg2> number of potions and places them in \<arg1>

---

#### <u> spl</u>

Casts the current spell

---

#### <u>clr \<arg1></u>

\<arg1>: Must either be the cauldron or a container

Empty \<arg1>


---

#### <u>inp \<arg1></u>

\<arg1>: Must be an empty container or a container with the same item

Takes all items from the input and moves them to \<arg1>

---

#### <u>out \<arg1> \<arg2></u>

\<arg1>: must be a non-empty container 
\<arg2>: **Optional**; number of items to move, by default its all item

Takes \<arg2> items from \<arg1> and places them onto the output