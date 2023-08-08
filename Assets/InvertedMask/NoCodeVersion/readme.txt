By using the two materials in this folder you
can achieve inverted masks without any scripting.

# Step 1 ##############

Make a hierarchy like this:

Canvas
  \- InvertedMask
      |- Hole (Image)
      \- Content (Image)

Make sure the "Hole" is above the "Content".

	  
# Step 2 ##############

Assign the "InvertedMaskContentMaterial" to the
Image Component of the "Content".

Assign the "InvertedMaskHoleMaterial" to the
Image Component of the "Hole".


# Step 3 ##############

You are done. There is nothing more to do.

You can find a demo of this setup under
Assets/InvertedMask/Examples/StaticDemo