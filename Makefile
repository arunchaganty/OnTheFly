TOMBOY_DIR=$(HOME)/.tomboy/addins
PKG_NAME=OnTheFly

OnTheFly.dll: OnTheFlyNoteAddin.cs OnTheFly.addin.xml
	gmcs -debug -out:OnTheFly.dll -define:DEBUG -target:library -pkg:tomboy-addins -r:Mono.Posix \
	OnTheFlyNoteAddin.cs -resource:OnTheFly.addin.xml \
	`pkg-config --libs tomboy-addins gnome-sharp-2.0`

install: OnTheFly.dll
	cp OnTheFly.dll $(TOMBOY_DIR)

uninstall:
	rm -vf $(TOMBOY_DIR)/OnTheFly.dll

package: 
	mkdir $(PKG_NAME)
	cp OnTheFlyNoteAddin.cs OnTheFly.addin.xml Makefile $(PKG_NAME)/
	tar -czf $(PKG_NAME).tar.gz $(PKG_NAME)/
	rm -rf $(PKG_NAME)

clean:
	rm -vf OnTheFly.dll OnTheFly.dll.mdb

.PHONY: package clean
