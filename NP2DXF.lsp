;-------------------------------------------------------------------------------------
;Main function
;script test
(vl-load-com)
(defun c:NP2DXF ()
  
   ;load the find and replace function and the text explode function
   (load "FindReplace")
   (load "txtexp")
	(print "Before SetOrigin")
   ;set the origin to lower left corner
   (SetOrigin)
	(print "Before Center")
   ;center the view in AutoCAD
   (Center )
   (print "Before getpartnum")
   ;get the part number for the blank plate
   (setq txt (GETPARTNUM ))
	(print "Before checkCTpartnum")
   (CheckCtPartNum)
   (print "Before getordernum")
   ;get the order number for the breakers
   (setq orderNum (tagval "NUMBER2"))
   (print "Before getbreakersnum")
   ;get how many breakers are on the order from the dialog
   (setq breakers (displayNumBreakers orderNum))
	(print "Before displaywarning")
   ;displays warning if the plate already exists
   (displayWarning)
	(print "Before Burstall")
   ;burst all of the blocks in the drawing to text
   (burstall)
  (print "Before cropNP txt")
   ;crop everything outside of the nameplate
   (CropNP txt)
  (print "Before setorigin2")
   ;set the origin to lower left corner
   (SetOrigin2)
	(print "Before Center")
   ;center the drawing
   (Center)
	;(print "Before HatchLogo")
   ;Convert Siemens Logo to hatch
   ;(HatchLogo)
	(print "Before CropBoundaries")
   ;Crop boundaries lines from the nameplate drawings
   (CropBoundaries)
	(print "Before -Scale")
   ;Scales the drawing by 10
   ;(command "DELAY" 1000)
   (-Scale)
	(print "Before SaveAsDXF")
   ;save each nameplate as a dxf file
   (SaveAsDXF orderNum txt breakers)
	(print "Before Center")
   ;center the drawing
   (Center)

   ;hide program output
   (print "Drawing for Order Completed")

)

;(defun Scale()
;	(command "SCALE" "ALL" "" '(0 0 0) 10)
;)

;-------------------------------------------------------------------------------------
;purpose: Show warning if a plate with part number already exists in folder to delete file first
(defun displayWarning ()
  	(setq SN (nth 0 (sparser orderNum "-")))
	(setq listFiles (vl-directory-files (strcat "\\\\ad101.siemens-energy.net\\dfs101\\file_se\\nam\\RCH_APPS\\Shared\\CroppedNameplates2\\" SN ) "*.dxf"))
	(setq iCount 0)
  	(setq sDWGNum (strcat txt ".dxf"))
  	(while (vl-string-position 45 sDWGNum) (setq sDWGNum (vl-string-subst "" "-" sDWGNum)))
  	(while (< iCount (length listFiles))
	  (setq sIndexFile (nth iCount listFiles))
	  (setq sIndexDrawingNum (nth 1 (sparser (nth iCount listFiles) "_")))
	  (if (= sIndexDrawingNum sDWGNum)
	    
	    (progn
		
	    	(alert (strcat "Warning: " sDWGNum " part number already exists in \\\\rchh220a\\Apps\\Shared\\CroppedNameplates2\\" SN "\n" "Please delete file from folder if the file is not meant to be there."))
	      
		;exits while
	        (setq iCount (length listFiles))
	      
		);end progn
	    );end if
	  
	  (setq iCount (+ iCount 1))
	);end while
)

;-------------------------------------------------------------------------------------
;purpose: burst CAT_ID tag blocks into text
(defun burstall ()

   (while (setq sset (ssget "X" '((0 . "INSERT"))))
     ;(setq sset (ssget "_x" '((0 . "INSERT")))) ;(2 . "LOGO"))))

      (sssetfirst nil sset)
      (c:burst)

   )
   (setq ssHatch (ssget "_C" '(-1.1033 11.3143 0.0) '(9.4327 -4.2472 0.0) '((0 . "HATCH"))))
   (if ssHatch (command "_.EXPLODE" ssHatch ""))
   (setq ssHatch2 (ssget "_C" '(-1.1033 11.3143 0.0) '(9.4327 -4.2472 0.0) '((0 . "HATCH"))))
   (if ssHatch2 (command "_.EXPLODE" ssHatch2 ""))
)

;-------------------------------------------------------------------------------------
;purpose: center the view of the drawing
(defun Center ()

  (command "zoom" "e")

)

;-------------------------------------------------------------------------------------
;purpose: scale the drawing
(defun -Scale ()

  (command "zoom" "e")

  (setq lowerLeft (list (nth 0 (getvar "extmin"))(nth 1 (getvar "extmin"))))
  (setq upperRight (list (nth 0 (getvar "extmax"))(nth 1 (getvar "extmax"))))
  (setq SizeX (- (nth 0 UpperRight) (nth 0 LowerLeft)))
  (setq SizeY (- (nth 1 UpperRight) (nth 1 LowerLeft)))
  (setq ScaleFactor (/ 80 SizeX))
  (command "SCALE" "ALL" "" '(0 0 0) ScaleFactor)

  (command "zoom" "e")

   
)

;-------------------------------------------------------------------------------------
;purpose: get coordinates according to plate number and crop everything outside those
;         coordinates
(defun CropNP (txt / ss1 s1 s2)
  
  ;if breaker nameplate make a crop in case there are tag number instructions
  (if (or (= txt "72-200-183-007") (= txt "72-200-184-027"))
	
	(progn
	  	(setq ss12 (ssget "_C" '(14.1352 11.1751 0.0) '(-4.1095 14.1239 0.0)))
	     	(if ss12
	       		(progn
			 	(setq ss13 (ssget "_C" '(-5.9655 13.7680 0.0) '(18.2664 -1.3257 0.0)))
		 		(command "_.SCALE" ss13 "" '(0.0 0.0 0.0) "0.833")
	       		);progn
	  	);if

	  
	        (setq ss1 (ssget "_W" '(0.74 4.93) '(7.75 4.6) '((0 . "TEXT"))))
		(if ss1 (command "_.Erase" ss1 ""))
		(prompt "\ntag notes deleted\n")
	);progn
  );if
  
  ;if txt matches the part number set the coordinates to be cropped
  ;  if no match is found give an alert
  (cond
     ((= txt "72-200-183-007") (setq pt1 '(0.42 10.75 0.0) pt2 '(8.05 4.79 0.0)))
     ((= txt "72-200-184-027") (setq pt1 '(0.4 10.72 0.0) pt2 '(7.9 4.79 0.0)))
     ((= txt "72-200-183-025") (setq pt1 '(2.2 8.9 0.0) pt2 '(7.5 3.5 0.0)))
     ((= txt "72-200-184-029") (setq pt1 '(2.2 8.9 0.0) pt2 '(7.2 3.5 0.0)))
     ((= txt "72-200-183-030") (setq pt1 '(0.5 8.0 0.0) pt2 '(7.0 3.8 0.0)))
     ((= txt "72-200-184-031") (setq pt1 '(0.5 8.0 0.0) pt2 '(7.0 3.8 0.0)))
     ((= txt "72-200-183-050") (setq pt1 '(0.55 8.0 0.0) pt2 '(8.0 6.0 0.0)))
     ((= txt "72-200-184-037") (setq pt1 '(0.6 8.35 0.0) pt2 '(8.0 5.9 0.0)))
     ((= txt "72-200-183-003") (setq pt1 '(0.95 2.5 0.0) pt2 '(7.2 8.5 0.0)))
     ((= txt "72-200-184-026") (setq pt1 '(0.5 8.2574 0.0) pt2 '(7.0019 3.4238 0.0)));(setq pt1 '(1.6 7.4 0.0) pt2 '(6.9 3.1 0.0))
     ((= txt "72-200-183-009") (setq pt1 '(0.53 8.86 0.0) pt2 '(7.1 2.93 0.0)))
     ((= txt "72-200-184-028") (setq pt1 '(0.53 8.86 0.0) pt2 '(7.1 2.93 0.0)))
     ((= txt "72-200-183-050") (setq pt1 '(0.5 8.2 0.0) pt2 '(8.0 5.8 0.0)))
     ((= txt "72-200-183") (setq pt1 '(1.0340 8.2574 0.0) pt2 '(7.0019 3.4238 0.0)))
     (t (progn (alert "Plate part number not recognized. Please notify, so the plate part numbers can be updated.") (exit)))
    );end cond
        
        ;select the area and delete all outside of the box
	(if (setq s1 (ssget "W" pt1 pt2))
	  (progn
      		(setq s2 (ssget "_X"))
      		(repeat (setq i (sslength s1))
        	  (ssdel (ssname s1 (setq i (1- i))) s2)
      		);end repeat
      		(repeat (setq i (sslength s2))
        	  (entdel (ssname s2 (setq i (1- i))))
      		);end repeat 
          );end progn
        )

  ;remove any remaining rotated dimensions
  (if (setq ssDimension (ssget "_X" '((0 . "DIMENSION"))))
  	(command "ERASE" ssDimension "")
  );end if
  
  (princ)
  
)

;--------------------------------------------------------------------------------------------------------------------------------
;purpose: cut boundaries and plate holes from nameplate drawing
;pt1 to pt2 is the window of the first hole. pt3 to pt4 is the  window the second hole and so on...
(defun CropBoundaries ( / ss1 ss2 ss3 ss4 ss5 ss6 ss7 ss8 ss9 ss10 ss11 ss12 ss13 pt1 pt2 pt3 pt4 pt5 pt6 pt7 pt8 pt9 pt10 pt11 pt12 drawingNum)

  (cond
	 ;breaker plates are 007 and 027
     ((= txt "72-200-183-007") (setq pt1 '(0.5 5.2 0.0) pt2 '(-0.2 5.8 0.0) pt3 '(7.2 5.2 0.0) pt4 '(6.4 5.8 0.0) pt5 '(7.1 -0.1 0.0) pt6 '(6.6 0.49 0.0) pt7 '(0.5 -0.1 0.0) pt8 '(-0.1 0.5 0.0) pt9 '(3.3 4.59 0.0) pt10 '(1.7 4.62 0.0) pt11 '(1.9518 5.0120 0.0) pt12 '(2.8986 4.6608 0.0)))
     ((= txt "72-200-184-027") (setq pt1 '(0.5 5.2 0.0) pt2 '(-0.2 5.8 0.0) pt3 '(7.2 5.2 0.0) pt4 '(6.4 5.8 0.0) pt5 '(7.1 -0.1 0.0) pt6 '(6.6 0.49 0.0) pt7 '(0.5 -0.1 0.0) pt8 '(-0.1 0.5 0.0) pt9 '(3.3 4.59 0.0) pt10 '(1.7 4.62 0.0) pt11 '(3.1 4.6 0.0) pt12 '(2.35 4.7 0.0)))
     ((= txt "72-200-183-025") (setq pt1 '(0.4 4.5 0.0) pt2 '(-0.2 5.1 0.0) pt3 '(4.6 4.5 0.0) pt4 '(4.1 5.0 0.0) pt5 '(4.7 -0.1 0.0) pt6 '(4.1 0.4 0.0) pt7 '(0.4 -0.1 0.0) pt8 '(-0.2 0.4 0.0)))
     ((= txt "72-200-184-029") (setq pt1 '(0.4 4.5 0.0) pt2 '(-0.2 5.1 0.0) pt3 '(4.6 4.5 0.0) pt4 '(4.1 5.0 0.0) pt5 '(4.7 -0.1 0.0) pt6 '(4.1 0.4 0.0) pt7 '(0.4 -0.1 0.0) pt8 '(-0.2 0.4 0.0)))
     ((= txt "72-200-183-030") (setq pt1 '(0.4 3.4 0.0) pt2 '(-0.2 4.0 0.0) pt3 '(6.2 3.4 0.0) pt4 '(5.5 4.0 0.0) pt5 '(6.2 -0.1 0.0) pt6 '(5.6 0.4 0.0) pt7 '(0.4 -0.1 0.0) pt8 '(-0.1 0.4 0.0)))
     ((= txt "72-200-184-031") (setq pt1 '(0.4 3.4 0.0) pt2 '(-0.2 4.0 0.0) pt3 '(6.2 3.4 0.0) pt4 '(5.5 4.0 0.0) pt5 '(6.2 -0.1 0.0) pt6 '(5.6 0.4 0.0) pt7 '(0.4 -0.1 0.0) pt8 '(-0.1 0.4 0.0)))
     ((= txt "72-200-183-050") (setq pt1 '(0.45 -0.3 0.0) pt2 '(-0.25 2.0 0.0) pt3 '(7.3 -0.2 0.0) pt4 '(6.6 1.2 0.0)))
     ((= txt "72-200-184-037") (setq pt1 '(0.45 -0.3 0.0) pt2 '(-0.25 2.0 0.0) pt3 '(7.3 -0.2 0.0) pt4 '(6.6 1.2 0.0)))
     ((= txt "72-200-183-003") (setq pt1 '(0.49 3.75 0.0) pt2 '(-0.2 4.3 0.0) pt3 '(5.26 4.2 0.0) pt4 '(4.63 3.7 0.0) pt5 '(4.69 0.39 0.0) pt6 '(5.33 -0.17 0.0) pt7 '(-0.26 -0.26 0.0) pt8 '(0.47 0.42 0.0)))
     ((= txt "72-200-184-026") (setq pt1 '(0.49 3.75 0.0) pt2 '(-0.2 4.3 0.0) pt3 '(5.26 4.2 0.0) pt4 '(4.63 3.7 0.0) pt5 '(4.69 0.39 0.0) pt6 '(5.33 -0.17 0.0) pt7 '(-0.26 -0.26 0.0) pt8 '(0.47 0.42 0.0)))
     ;((= txt "72-200-184-026") (setq pt1 '(0.4 3.7 0.0) pt2 '(-0.2 4.4 0.0) pt3 '(5.3 3.7 0.0) pt4 '(4.5 4.4 0.0) pt5 '(5.3 -0.2 0.0) pt6 '(4.7 0.4 0.0) pt7 '(0.4 -0.1 0.0) pt8 '(-0.1 0.4 0.0)))
     ((= txt "72-200-183-009") (setq pt1 '(-0.16 5.82 0.0) pt2 '(0.7 5.26 0.0) pt3 '(-0.122 0.358 0.0) pt4 '(0.426 -0.158 0.0) pt5 '(6.048 0.295 0.0) pt6 '(6.50 -0.084 0.0) pt7 '(5.985 5.85 0.0) pt8 '(6.458 5.387 0.0)))
     ((= txt "72-200-184-028") (setq pt1 '(-0.16 5.82 0.0) pt2 '(0.7 5.26 0.0) pt3 '(-0.122 0.358 0.0) pt4 '(0.426 -0.158 0.0) pt5 '(6.048 0.295 0.0) pt6 '(6.50 -0.084 0.0) pt7 '(5.985 5.85 0.0) pt8 '(6.458 5.387 0.0)))
     ((= txt "72-200-183-050") (setq pt1 '(-0.3 2 0.0) pt2 '(0.55 -0.33 0.0) pt3 '(6.4 2.0 0.0) pt4 '(7.3 -0.33 0.0) pt5 '(-0.3 2.0 0.0) pt6 '(0.55 -0.33 0.0) pt7 '(6.4 2.0 0.0) pt8 '(7.3 -0.33 0.0)))
     (t nil)
    );end cond

    (setq drawingNum (getvar "dwgname"))
    (setq drawingNum (vl-string-subst "" ".dwg" drawingNum))

    ;if breaker nameplate make a crop in case there are tag number instructions
    (if (= drawingNum "72184087091")

      ;progn for if statement is True
      (progn
	(setq pt1 '(0.3 4.1 0.0) pt2 '(-0.2 4.4 0.0))
	(setq ss1 (ssget "_C" pt1 pt2 '((0 . "*POLYLINE"))))
        (if ss1 (command "_.Erase" ss1 ""))
	);progn

      ;progn for if statement is False
      (progn
	    (if (or (= txt "72-200-183-050") (= txt "72-200-184-037"))

		(progn
		     (setq ss1 (ssget "_C" pt1 pt2 '((0 . "LINE"))))
	             (if ss1 (command "_.Erase" ss1 ""))
		     (setq ss5 (ssget "_C" pt1 pt2 '((0 . "CIRCLE"))))
	             (if ss5 (command "_.Erase" ss5 ""))
		     (setq ss5 (ssget "_C" pt1 pt2 '((0 . "INSERT"))))
	             (if ss5 (command "_.Erase" ss5 ""))
		  
		     (setq ss2 (ssget "_C" pt3 pt4 '((0 . "LINE"))))
	             (if ss2 (command "_.Erase" ss2 ""))
		     (setq ss6 (ssget "_C" pt3 pt4 '((0 . "CIRCLE"))))
	             (if ss6 (command "_.Erase" ss6 ""))
		     (setq ss6 (ssget "_C" pt3 pt4 '((0 . "INSERT"))))
	             (if ss6 (command "_.Erase" ss6 ""))
		  
		);progn

	        (progn
			 (setq ss1 (ssget "_C" pt1 pt2 '((0 . "LINE"))))
	             (if ss1 (command "_.Erase" ss1 ""))
		     (setq ss5 (ssget "_C" pt1 pt2 '((0 . "CIRCLE"))))
	             (if ss5 (command "_.Erase" ss5 ""))
		     (setq ss5 (ssget "_C" pt1 pt2 '((0 . "INSERT"))))
	             (if ss5 (command "_.Erase" ss5 ""))
		  
		     (setq ss2 (ssget "_C" pt3 pt4 '((0 . "LINE"))))
	             (if ss2 (command "_.Erase" ss2 ""))
		     (setq ss6 (ssget "_C" pt3 pt4 '((0 . "CIRCLE"))))
	             (if ss6 (command "_.Erase" ss6 ""))
		     (setq ss5 (ssget "_C" pt1 pt2 '((0 . "INSERT"))))
	             (if ss5 (command "_.Erase" ss5 ""))
		  
		     (setq ss3 (ssget "_C" pt5 pt6 '((0 . "LINE"))))
	             (if ss3 (command "_.Erase" ss3 ""))
	             (setq ss7 (ssget "_C" pt5 pt6 '((0 . "CIRCLE"))))
	             (if ss7 (command "_.Erase" ss7 ""))
		     (setq ss5 (ssget "_C" pt1 pt2 '((0 . "INSERT"))))
	             (if ss5 (command "_.Erase" ss5 ""))
		     
		     (setq ss4 (ssget "_C" pt7 pt8 '((0 . "LINE"))))
	             (if ss4 (command "_.Erase" ss4 ""))
		     (setq ss8 (ssget "_C" pt7 pt8 '((0 . "CIRCLE"))))
	             (if ss8 (command "_.Erase" ss8 ""))
		     (setq ss5 (ssget "_C" pt1 pt2 '((0 . "INSERT"))))
	             (if ss5 (command "_.Erase" ss5 ""))
		     
		);progn
	    );if
      );progn
    );if
  
    ;crop any stickers left on the nameplate before saving
    (if (or (= txt "72-200-184-027") (= txt "72-200-183-007"))
      
	(progn
	     
	  
	     ;(setq ss9 (ssget "_C" pt9 pt10 '((0 . "LINE"))))
             ;(if ss9 (command "_.Erase" ss9 ""))
	     ;(setq ss10 (ssget "_C" pt11 pt12 '((0 . "LINE"))))
             ;(if ss10 (command "_.Erase" ss10 ""))
	  
	);progn
      
    );if
  
    ;justifies all CT text to the same alignment to prevent LaserCAD importing issues 
   
      	 (progn

	   (setq ss11 (ssget "X" '((0 . "TEXT"))))
	   (if ss11 (command "_.JUSTIFYTEXT" ss11 "" "MC"))

	 );progn

    
    ;(if (or (= txt "72-200-183-050") (= txt "72-200-184-037"))
    ; (progn
	
	;setq ssSeismic (ssget "_C" '(-1.0 2.0 0.0) '(2.5 -0.5 0.0) '((0 . "TEXT"))))
	
	;)
     ;)


);defun

;-------------------------------------------------------------------------------------
;purpose: save a dxf for each breaker needed for the current drawing
(defun SaveAsDxf(orderNum txt breakers / slen specOrderNum count)
  
   
   ;take the dashes out of the plate part number
   (while (vl-string-position 45 txt) (setq txt (vl-string-subst "" "-" txt)))
   
   ;set the count to the first number on the stock order
   (setq count (atoi firstBreaker))
	
   (setq breakers (itoa breakers))
   
   ;EMK 11/2021 Added check for serial number length rather than assume 10
	(setq orderNumDashless (nth 0 (sparser orderNum "-")))  
   
  
   ;set array index to 0
   (setq index 0)

   ;if there is only one breaker on the drawing, do not loop
   (if (= breakers "1")
	(progn

           ;if the stock order number folder does not exist, create it
           ;(setq filePath2 (strcat "\\\\ad101.siemens-energy.net\\dfs101\\file_se\\nam\\RCH_APPS\\Shared\\CroppedNameplates2\\" (substr orderNum 1 10) "\\"))
		   (setq filePath2 (strcat "\\\\ad101.siemens-energy.net\\dfs101\\file_se\\nam\\RCH_APPS\\Shared\\CroppedNameplates2\\" orderNumDashless "\\"))
	`		
	   ;make the file path
	   (vl-mkdir filePath2)
 
	   ;format the strings for saving the file
           (setq currentFileName (getvar "dwgname"))
	  
	   
	     
           (setq currentFileName (vl-string-subst "" ".dwg" currentFileName))

	   (setq currentFileName (substr currentFileName 1 11))
	  
	   ;if drawing name is shorter than 11 characters, the robot program will not accept it, therefore any drawing number is extended to 11 characters here
	   (while (< (strlen currentFileName) 11) (setq currentFileName (strcat "1" currentFileName)))
	  
           (setq newFileName (strcat currentFileName "_" txt))
           ;(setq specOrderNum (strcat (substr orderNum 1 10) "-" (itoa count)))
		   (setq specOrderNum (strcat orderNumDashless "-" (itoa count)))

	   ;find the general stock order text and replace with specific order number
           (if (and (/= txt "72200184031")(/= txt "72200183030")(/= txt "72200183009")(/= txt "72200184028")) (tfindfun orderNum (strcat specOrderNum) 0))

           ;change the date on the nameplate
	   ;blank needs to be "Month/Year"
           (changeDate mfrDate)

	   ;if customer breaker IDs were enter in the text box,
	   ;  place them in the correct box on the drawing
	   (if (/= bIDs nil) (EDITBREAKERID bIDs))

	   ;set the save path
	   (setq saveAsPath2 (strcat filePath2 "(" (itoa count) ")" newFileName ".dxf"))

	   (if (= tog2val "1")
	     (progn
	     (setq saveAsPath2 (strcat filePath2 "(" (itoa count) ")" newFileName "&QR.dxf"))
	     );progn (if)
	   )
	  
	   ;if multiple operator plates are needed
	   (if (and (= tog4val "1") (or (= txt "72200184029") (= txt "72200183025")))
		  
		(progn
			(tfindfun specOrderNum (strcat specOrderNum " A" ) 0)
			(setq newFileName2 (vl-string-subst "A_" "_" newFileName))
			(setq saveAsPath2 (strcat filePath2 "(" (itoa count) ")" newFileName2 ".dxf"))
			;(setq saveAsPath2 (vl-string-subst "A_" "_" saveAsPath2))
			(TEXT_TO_LINE)
			(overwrite saveAsPath2)
			(vla-saveas (vla-get-ActiveDocument (vlax-get-Acad-Object)) saveAsPath2 acR15_DXF)
			(command "UNDO" 2)
			
			(tfindfun (strcat specOrderNum " A" ) (strcat specOrderNum " B" ) 0)
			(setq newFileName2 (vl-string-subst "B_" "_" newFileName))
			(setq saveAsPath2 (strcat filePath2 "(" (itoa count) ")" newFileName2 ".dxf"))
			(TEXT_TO_LINE)
			(overwrite saveAsPath2)
			(vla-saveas (vla-get-ActiveDocument (vlax-get-Acad-Object)) saveAsPath2 acR15_DXF)
			(command "UNDO" 2)
			
			(tfindfun (strcat specOrderNum " B" ) (strcat specOrderNum " C" ) 0)
			(setq newFileName2 (vl-string-subst "C_" "_" newFileName))
			(setq saveAsPath2 (strcat filePath2 "(" (itoa count) ")" newFileName2 ".dxf"))
			;(setq saveAsPath2 (vl-string-subst "C_" "B_" saveAsPath2))
			(TEXT_TO_LINE)
			(overwrite saveAsPath2)
			(vla-saveas (vla-get-ActiveDocument (vlax-get-Acad-Object)) saveAsPath2 acR15_DXF)
			(command "UNDO" 2)
		);progn(if)
		  
		(progn
		  	(TEXT_TO_LINE)
		  	(overwrite saveAsPath2)
   			(vla-saveas (vla-get-ActiveDocument (vlax-get-Acad-Object)) saveAsPath2 acR15_DXF)
		  	(command "UNDO" 2)
		);progn(else)
	   );if

        );progn

	;else loop for the number of breakers called for on the drawing
        (progn 
		
   		;if the stock order number folder does not exist, create it
   		;(setq filePath2 (strcat "\\\\ad101.siemens-energy.net\\dfs101\\file_se\\nam\\RCH_APPS\\Shared\\CroppedNameplates2\\" (substr orderNum 1 10) "\\"))
		(setq filePath2 (strcat "\\\\ad101.siemens-energy.net\\dfs101\\file_se\\nam\\RCH_APPS\\Shared\\CroppedNameplates2\\" orderNumDashless "\\"))
	  	(vl-mkdir filePath2)

		;get the current filename
   		(setq currentFileName (getvar "dwgname"))
	  	
		
		;take off the ".dwg" filetype
   		(setq currentFileName (vl-string-subst "" ".dwg" currentFileName))
	  	;if drawing name is shorter than 11 characters, the robot program will not accept it, therefore any drawing number is extended to 11 characters here
		(while (< (strlen currentFileName) 11) (setq currentFileName (strcat "1" currentFileName)))
		;set the new file name to be the drawing name then blank nameplate number
   		(setq newFileName (strcat currentFileName "_" txt))

		;set the specific breaker order number
   		;(setq specOrderNum (strcat (substr orderNum 1 10) "-" (itoa count)))
		(setq specOrderNum (strcat orderNumDashless "-" (itoa count)))
		
		;find the generic order number and replace with the specific order number
   		;(if (and (/= txt "72-200-184-030")(/= txt "72-200-183-031")) (tfindfun specOrderNum (strcat (substr orderNum 1 10) "-" (itoa count)) 0));
	  	(if (and (/= txt "72200184031")(/= txt "72200183030")(/= txt "72200183009")(/= txt "72200184028")) (tfindfun orderNum (strcat specOrderNum) 0))

		;change the date on the drawing "Month/Year"
	  	(changeDate mfrDate)
		
		;if customer breaker IDs were entered in the dialog, add them to each DXF
		(if (/= bIDs nil) 
			(progn 
				(setq bIdList (sparser bIDs ",")) 
				(EDITBREAKERID (nth index bIdList))
			)
		)
				
		;set the save path for the file
	  	(setq saveAsPath2 (strcat filePath2 "(" (itoa count) ")" newFileName ".dxf"))

	  	(if (= tog2val "1")

		  (progn
		  	(setq saveAsPath2 (strcat filePath2 "(" (itoa count) ")" newFileName "&QR.dxf"))
		  );progn (if)
	  	)

	  	;if multiple operator plates are needed
	  	(if (and (= tog4val "1") (or (= txt "72200184029") (= txt "72200183025")))
		  
			(progn
		  		(tfindfun specOrderNum (strcat specOrderNum " A" ) 0)
				(setq newFileName2 (vl-string-subst "A_" "_" newFileName))
				(setq saveAsPath2 (strcat filePath2 "(" (itoa count) ")" newFileName2 ".dxf"))
			  	;(setq saveAsPath2 (vl-string-subst "A_" "_" saveAsPath2))
			  	(TEXT_TO_LINE)
			  	(overwrite saveAsPath2)
   				(vla-saveas (vla-get-ActiveDocument (vlax-get-Acad-Object)) saveAsPath2 acR15_DXF)
			  	(command "UNDO" 2)
                
		  		(tfindfun (strcat specOrderNum " A" ) (strcat specOrderNum " B" ) 0)
				(setq newFileName2 (vl-string-subst "B_" "_" newFileName))
				(setq saveAsPath2 (strcat filePath2 "(" (itoa count) ")" newFileName2 ".dxf"))
			  	(TEXT_TO_LINE)
			    (overwrite saveAsPath2)
   				(vla-saveas (vla-get-ActiveDocument (vlax-get-Acad-Object)) saveAsPath2 acR15_DXF)
			  	(command "UNDO" 2)
                
		  		(tfindfun (strcat specOrderNum " B" ) (strcat specOrderNum " C" ) 0)
				(setq newFileName2 (vl-string-subst "C_" "_" newFileName))
				(setq saveAsPath2 (strcat filePath2 "(" (itoa count) ")" newFileName2 ".dxf"))
			  	;(setq saveAsPath2 (vl-string-subst "C_" "B_" saveAsPath2))
			  	(TEXT_TO_LINE)
			  	(overwrite saveAsPath2)
   				(vla-saveas (vla-get-ActiveDocument (vlax-get-Acad-Object)) saveAsPath2 acR15_DXF)
			  	(command "UNDO" 2)
			  
			  	(tfindfun (strcat specOrderNum " C") specOrderNum 0)
			);progn(if)
		  
		  	(progn
			  	(TEXT_TO_LINE)
			  	(overwrite saveAsPath2)
   				(vla-saveas (vla-get-ActiveDocument (vlax-get-Acad-Object)) saveAsPath2 acR15_DXF)
			  	(command "UNDO" 2)	
			);progn(else)
		);if


		;repeat the steps above for the number of breakers that were on the drawing
   		(repeat (- (atoi breakers) 1)

       		  (setq count (1+ count))

		  (setq index (1+ index))

		  (if (/= bIDs nil) (EDITBREAKERID (nth index bIdList)))

       		  ;(if (and (/= txt "72200184031")(/= txt "72200183030")(/= txt "72200183009")(/= txt "72200184028")) (tfindfun specOrderNum (strcat (substr orderNum 1 10) "-" (itoa count)) 0));(tfindfun orderNum (strcat specOrderNum) 0)
				(if (and (/= txt "72200184031")(/= txt "72200183030")(/= txt "72200183009")(/= txt "72200184028")) (tfindfun specOrderNum (strcat orderNumDashless "-" (itoa count)) 0))

       		  ;(setq specOrderNum (strcat (substr orderNum 1 10) "-" (itoa count)))
			  (setq specOrderNum (strcat orderNumDashless "-" (itoa count)))
       
		  (setq saveAsPath2 (strcat filePath2 "(" (itoa count) ")" newFileName ".dxf"))

		  (if (= tog2val "1")

		    (progn
		    	(setq saveAsPath2 (strcat filePath2 "(" (itoa count) ")" newFileName "&QR.dxf"))
		    );progn (if)
		    
	  	  )

			 ;if multiple operator plates are needed
			(if (and (= tog4val "1") (or (= txt "72200184029") (= txt "72200183025")))
			  
				(progn
					(tfindfun specOrderNum (strcat specOrderNum " A" ) 0)
					(setq newFileName2 (vl-string-subst "A_" "_" newFileName))
					(setq saveAsPath2 (strcat filePath2 "(" (itoa count) ")" newFileName2 ".dxf"))
					;(setq saveAsPath2 (vl-string-subst "A_" "_" saveAsPath2))
					(TEXT_TO_LINE)
					(overwrite saveAsPath2)
					(vla-saveas (vla-get-ActiveDocument (vlax-get-Acad-Object)) saveAsPath2 acR15_DXF)
					(command "UNDO" 2)
					
					(tfindfun (strcat specOrderNum " A" ) (strcat specOrderNum " B" ) 0)
					(setq newFileName2 (vl-string-subst "B_" "_" newFileName))
					(setq saveAsPath2 (strcat filePath2 "(" (itoa count) ")" newFileName2 ".dxf"))
					(TEXT_TO_LINE)
					(overwrite saveAsPath2)
					(vla-saveas (vla-get-ActiveDocument (vlax-get-Acad-Object)) saveAsPath2 acR15_DXF)
					(command "UNDO" 2)
					
					(tfindfun (strcat specOrderNum " B" ) (strcat specOrderNum " C" ) 0)
					(setq newFileName2 (vl-string-subst "C_" "_" newFileName))
					(setq saveAsPath2 (strcat filePath2 "(" (itoa count) ")" newFileName2 ".dxf"))
					;(setq saveAsPath2 (vl-string-subst "C_" "B_" saveAsPath2))
					(TEXT_TO_LINE)
					(overwrite saveAsPath2)
					(vla-saveas (vla-get-ActiveDocument (vlax-get-Acad-Object)) saveAsPath2 acR15_DXF)
					(command "UNDO" 2)
				  
					(if (/= (atoi breakers) (+ 1 index)) (tfindfun (strcat specOrderNum " C") specOrderNum 0))
				);progn(if)
			  
				(progn
					(TEXT_TO_LINE)
					(overwrite saveAsPath2)
					(vla-saveas (vla-get-ActiveDocument (vlax-get-Acad-Object)) saveAsPath2 acR15_DXF)
					(command "UNDO" 2)	
				);progn(else)
			);if
       
   		  );repeat

   	);progn

   );if
  
)

;-------------------------------------------------------------------------------------

(defun SetOrigin ()

  (if (and 
	   (setq ss (ssget "_X"))
	   (setq lowerLeft (list (nth 0 (getvar "extmin"))(nth 1 (getvar "extmin"))))
      );end and
    (command "_move" ss "" "_NONE" lowerLeft "_NONE" '(0.0 0.0 0.0))
  );end if

)

;-------------------------------------------------------------------------------------

(defun SetOrigin2 ()

    (command "zoom" "object" "all" "") 
    (command "move" "all" "" (getvar "extmin") '(0 0))
  
)

;-------------------------------------------------------------------------------------
;purpose: if the part num text is found use that, if not then search for an attribute tag
(defun GETPARTNUM  ()

  (if (setq tss (ssget "X" '((1 . "72-200-*"))))
    
    (setq myStr (cdr (assoc 1 (entget (ssname tss 0)))))
    
    (setq myStr (GETTAG))
    
  );if

);end defun

;-------------------------------------------------------------------------------------
;purpose: find the attribute tag with the nameplate part number
(defun GETTAG ( / pt1 pt2)

  ;set area to select
  (setq pt1 '(2.4341 2.9883));2.5222 2.6888
  (setq pt2 '(4.5722 2.1747));4.0403 2.2279

  ;select the area
  (setq asel (ssget "W" pt1 pt2 '((0 . "ATTDEF"))))

  ;set the object for the tag
  (setq aobj (vlax-ename->vla-object (ssname asel 0)))
  (setq tag (vla-get-TagString aobj))

);end defun

;-------------------------------------------------------------------------------------
;purpose: get the last character of a string
(defun getlastchar (str)

    (setq slen (strlen str))

    (setq lastchar (substr str slen 1))

)

;-------------------------------------------------------------------------------------
;purpose: get the value of a tag: NOT USED
(defun tagval (tag / ent hnd i lst ss)
  (if (setq ss (ssget "_X" (list '(0 . "INSERT") '(66 . 1) (cons 410 (getvar 'CTAB)))))
    (repeat (setq i (sslength ss))
      (setq hnd (ssname ss (setq i (1- i))))
      (while (and
               (setq hnd (entnext hnd))
               (setq ent (entget hnd))
               (= (cdr (assoc 0 ent)) "ATTRIB")
             )
        (if (= (strcase tag) (strcase (cdr (assoc 2 ent))))
          (setq lst (cons (cdr (assoc 1 ent)) lst))
        )
      )
    )
  )
  (if lst
    (car lst)
  )
)

;--------------------------------------------------------------------------------------
;purpose: change the date on the drawing
(defun changeDate (mfrDate)

   ;get current date
   ;(setq cdate_val (rtos (getvar "CDATE")))
   ;(setq YYYY (substr cdate_val 1 4)
   ;      M    (substr cdate_val 5 2)
   ;)
   
   ;format the string for the month and year
   ;(setq MMYYYY (strcat M "/" YYYY))

   ;find "Month/Year" and replace with current month and year
   (tfindfun "Month/Year" mfrDate 0)
   (tfindfun "Month / Year" mfrDate 0)
   (tfindfun "MONTH / YEAR" mfrDate 0)
   (tfindfun "MONTH/YEAR" mfrDate 0)
   (tfindfun "MM/YYYY" mfrDate 0)
   (tfindfun "MM / YYYY" mfrDate 0 )
   (tfindfun "MONTH/DATE" mfrDate 0)
   (tfindfun "Month/Date" mfrDate 0)
   (tfindfun "MONTH / DATE" mfrDate 0)
   (tfindfun "Month / Date" mfrDate 0)

)

;-----------------------------------------------------------------------------------------

;fucntion: display number of proposed files to create, then have the user
;           input the correct number of DXF files to make,
;           and the number of the first breaker for the set of drawings,
;           the user will also enter customer breaker IDs if applicable
;
;accepts: orderNum - generic order number for determining number of breakers on drawing

(defun displayNumBreakers ( orderNum / customerOrder )

   ;find the likely number of breakers on the drawing 
   (setq slen (strlen orderNum))
   (setq position (vl-string-search "-" orderNum))
   
   (setq numOfOrdersSubStr (substr orderNum (+ position 2) slen))
   (setq customerOrder (substr orderNum 1 (+ position 1)))
   (setq numOfOrdersList (sparser numOfOrdersSubStr ","))
   (setq numOfOrdersList2 (sparser numOfOrdersSubStr " "))
  
   
   (setq numOfOrders (itoa (vl-list-length numOfOrdersList)))
   (setq firstBreaker (nth 0 numOfOrdersList))
  
   (if (= (vl-list-length numOfOrdersList2) 3)
     
	(progn
     		(setq firstBreaker (nth 0 numOfOrdersList2))
     		(setq numOfOrders (itoa (+ (- (atoi (last numOfOrdersList2)) (atoi (nth 0 numOfOrdersList2))) 1)))
        );progn
     
   );end if
   
   (setq lastnum (- (+ (atoi firstBreaker) (atoi numOfOrders)) 1))
   
  
   ;create the string to display number of drawings to create
   (setq msg (strcat "The program will create " numOfOrders " DXF files,"))
   (setq msg2 (strcat "with orders from " (strcat customerOrder firstBreaker)  " to " (strcat customerOrder (itoa lastnum)) "." ))
  
   (setq cdate_val (rtos (getvar "CDATE")))
   (setq YYYY (substr cdate_val 1 4)
         M    (substr cdate_val 5 2)
   )
   
   ;format the string for the month and year
   (setq MMYYYY (strcat M "/" YYYY))

   ;initialize the dialog
   (setq dcl_id (load_dialog "test_dcl.dcl"))
 
     (if (not (new_dialog "test_dcl" dcl_id))
	 (exit )
     );if
   
   ;display the message for number of drawings and instructions for filling the dialog
   (set_tile "breakers" msg)
   (set_tile "range" msg2)
   
   ;(set_tile "firstnum" firstBreaker)
  
   ;set the number of drawings that will be created in the edit box
   ;(set_tile "numenter" numOfOrders)
  
   ;set the first edit box to focus   
   ;(mode_tile "numenter" 2)
  
   ;set the text for the breaker ID text box
   (set_tile "bnums" "Enter Breaker ID's Here") ;set text
   
   ; set what happens when ok is selected
   (action_tile "accept" "(ErrorCheck)");action_tile

   
   ;get value from num of breakers text box
   (action_tile "numenter" "(setq numOfOrders $value)")
	
   ;get value from firstNum of breaker on the order
   (action_tile "firstnum" "(setq firstBreaker $value)")
   ;(setq firstNum (get_tile "firstnum"))

   
  
   ;get value from mfrDate of breaker on the order
   (action_tile "mfrDate" "(setq mfrDate $value)")
  
   ;set breaker id text box to toggle with the toggle switch
   (mode_tile "numenter" 1)
   (mode_tile "firstnum" 1)
   (action_tile "tog1" "(setq togval $value)
   			(setq boxOnOff (- 1 (atoi togval)))
   			(mode_tile \"bnums\" boxOnOff) (mode_tile \"bnums\" 2)")

   (action_tile "tog3" "(setq tog3val $value)
     			(setq boxOnOff2 (- 1 (atoi tog3val)))
     			(mode_tile \"numenter\" boxOnOff2)
     			(mode_tile \"firstnum\" boxOnOff2) ")

   ;get value from customer breaker id's text box
   (action_tile "bnums" "(setq bIDs $value)")

   (if (and (/= txt "72-200-184-027") (/= txt "72-200-183-007"))
     
     (if (or (= txt "72-200-184-029") (= txt "72-200-183-025"))
       
       (progn
	  (mode_tile "tog2" 1)
       );progn
       
       (progn
	  (mode_tile "tog2" 1)
	  (mode_tile "tog1" 1)
	  (mode_tile "para1" 1)
       );progn
       
     );if

   );if

   (if (and (/= txt "72-200-184-029") (/= txt "72-200-183-025"))
     
	(mode_tile "tog4" 1)

   );if

  ;Don't fill in date for CT plate
;;;     (if (or (= txt "72-200-184-031") (= txt "72-200-183-030")) 
;;;
;;;       (mode_tile "mfrDate" 1)
;;;
;;;     );if
  
   ;get value from the QR checkbox
   (action_tile "tog2" "(setq tog2val $value)")

   ;get value from the 3 operator checkbox
   (action_tile "tog4" "(setq tog4val $value)")

   ;program will immediately exit if the cancel button is selected
   (action_tile "cancel" "(done_dialog) (exit)")

   ;start the dialog process
   (start_dialog)

   ;unload the dialog
   (unload_dialog dcl_id)
 
   ;return the number of breakers the user input

  
;;;   (if (= tog3val "1")
;;;     
;;;     	(progn
;;;
;;;	  
;;;	  ;get value from num of breakers text box
;;;	  (princ (get_tile "numenter"))
;;;	  (action_tile "numenter" "(setq numOfOrders $value)")
;;;   	  ;get value from firstNum of breaker on the order
;;;	  (princ (get_tile "firstnum"))
;;;	  (action_tile "firstnum" "(setq firstBreaker $value)")
;;;	  (princ numOfOrders)
;;;	  (princ firstBreaker)
;;;	  
;;;	);progn
;;;
;;;   );if
  
  (setq returnVal (atoi numOfOrders))
   
)
(princ)

;---------------------------------------------------------------------------------------
;purpose: check if there are errors in the dialog input
(defun ErrorCheck ()

	;when OK is selected, if either of the edit boxes are empty,
        ; display the error and do nothing
	;
	;if ok is selected and both are filled the dialog will close
	(if (or (and (or (= (get_tile "firstnum") "") (= (get_tile "numenter") "")) (= tog3val "1")) (= (get_tile "mfrDate") ""))

		(progn
		    (set_tile "error" "You must fill in all applicable fields.")
		    (mode_tile "mfrDate" 2)
		    
		);progn
		
		(done_dialog)
	);if
  
;;;	(if (= (get_tile "mfrDate") "")
;;;
;;;		(progn
;;;		    (set_tile "error" "You must fill in date field.")
;;;		    (mode_tile "mfrDate" 2)
;;;		    (princ "mfrdate was empty")
;;;		    (setq temp (getstring))
;;;		);progn
;;;
;;;		(done_dialog)
;;;	);if
  	
  	;(mode_tile "firstnum" 0)
  	;(mode_tile "numenter" 0)
  
	;(mode_tile "firstnum" 2)
  	;(mode_tile "numenter" 2)
)

;---------------------------------------------------------------------------------------
;function: replace the prompt with breaker ID
;
;accepts: newID - customer breaker ID specified in dialog box
(defun EDITBREAKERID ( newID / ss ss1 ent elist ent1 elist1 pt1 pt2)

    ;find the text see below and replace that line of text
    (if (setq ss (ssget "X" '((1 . "*SEE BELOW*"))))
	(progn
        (setq ent (ssname ss 0)); entity
	    (setq elist (entget ent)); entity list
	    (if (= (cdr (assoc 0 elist)) "TEXT")
	        (progn
	            (entmod (setq elist (subst (cons 1 newID)(assoc 1 elist) elist)))
	        ); progn
        ); if

	    (setq oldID newID)
	    (prompt "\nin if:")
	    (princ oldID)
	
	);progn

	(progn
        (prompt "\nin else:")
	    (princ oldID)
	    (setq ss1 (ssget "X" (list (cons 1 oldID))))
	    (setq ent1 (ssname ss1 0)); entity
	    (setq elist1 (entget ent1)); entity list
	    (if (= (cdr (assoc 0 elist1)) "TEXT")
	        (progn
	    	    (entmod (setq elist1 (subst (cons 1 newID)(assoc 1 elist1) elist1)))
	        ); progn
        ); if

	    (setq oldID newID)

        );progn

    );if

    (princ)

);end defun

;----------------------------------------------------------------------------------------
;function: split string into list by delimiter
;
;accepts: str - string to be split
;         delim - character to split the string at
(defun sparser (str delim / ptr lst)
    (while (setq ptr (vl-string-search delim str))
	(setq lst (cons (substr str 1 ptr) lst))
	(setq str (substr str (+ ptr 2)))
    )
    (reverse (cons str lst))
);defun


;----------------------------------------------------------------------------------------
;function: check if file exist, delete before saving if the file does exist
;
;accepts: path of file to check and delete
(defun overwrite ( filePath / oldFilePath oldFilePath2 )

  	(if (= tog2val "1")
		(progn
			(setq oldFilePath (vl-string-subst "" "&QR" filePath))
	  		(if (findfile oldFilePath) (vl-file-delete oldFilePath))
		);progn
		  
	);if

  	;if there needs to be three operator plates
  	(if (= tog4val "1")
	  
		(progn
			(setq oldFilePath2 (vl-string-subst "_" "A_" filePath))
		    	(if (findfile oldFilePath2) (vl-file-delete oldFilePath2))
		);progn
		  
	);if

	(if (findfile filePath) (vl-file-delete filePath))
	

);defun

;-----------------------------------------------------------------------------------------
;function: check if the drawing number is 72183919039 and the part number is 72200184026
;          if so change the part number to 72200184031
(defun CheckCtPartNum ( / drawingNum )

  ;get the current drawing number
  (setq drawingNum (getvar "dwgname"))
  (setq drawingNum (vl-string-subst "" ".dwg" drawingNum))

  (if (and (or (= drawingNum "72183918109") (= drawingNum "72183919039")) (= txt "72-200-184-026")) (setq txt "72-200-184-031"))
  

);defun


(princ)

(defun DT:SendKeys (keys / ws) 
  (setq ws (vlax-create-object "WScript.Shell")) 
  (vlax-invoke-method ws 'sendkeys keys) 
  (vlax-release-object ws) 
  (princ) 
)

(defun chgwid4allplines  (width / sset)
  (vl-load-com)
  (if (setq sset (ssget "_x" (list (cons 0 "POLYLINE,LWPOLYLINE"))))
    (mapcar
     '(lambda (pline) (vla-put-constantwidth pline width))
      (mapcar
        'vlax-ename->vla-object
        (vl-remove-if 'listp (mapcar 'cadr (ssnamex sset)))
      )
    )
  )
  (princ)
)

;-----------------------------------------------------------------------------------------
;function: convert text to lines for CNC machine
(defun TEXT_TO_LINE () 
  (chgwid4allplines 0.005)
  ;(setq ss (ssget "X"))
  ;(setq sFile "\\\\rchh220a\\Apps\\Shared\\CroppedNameplates2\\TextToLine.wmf")
  ;(command "WMFOUT" sFile ss "") 
  ;(command "ERASE" ss "")
  ;(setq ss2 (ssget "_X"))
  ;(command "CUTCLIP" ss2 "")
  ;(command "WMFIN" sFile '(0.0 0.0 0.0) 2 2 0)
  ;(command "PASTECLIP" '(0.0 0.0 0.0) "")
  (setq ssText (ssget "_X" '((0 . "*TEXT"))))
  (command "PSELECT" ssText "")
  (txtexp)
  (Center )
  ;(command "EXPLODE" ss3)
  (setq ss4 (ssget "X"))
  (repeat (setq i (sslength ss4))
      (setpropertyvalue (ssname ss4 (setq i (1- i))) "LINEWEIGHT" 15)
  );end repeat
  (chgwid4allplines 0.01)
)

;-----------------------------------------------------------------------------------------
;function: converts Siemens Logo to a hatch
(defun HatchLogo()
  ;(setq ssBlock (ssget "_x" '((0 . "INSERT")))) ;(2 . "LOGO"))))
  
  (if (or (= txt "72-200-184-027") (= txt "72-200-183-007"))
	(if (= tog2val "1")
		(progn
			(setq pt1 '(2.25 4.60 0.0) pt2 '(4.375 5.5 0.0))	;CB QR
			;(setq pt1 '(2.5 4.6 0.0) pt2 '(4.375 5.5 0.0))
		)
		(progn
			(setq pt1 '(1 5.05 0.0) pt2 '(4.8 5.5 0.0))			;CB w/o QR
			(setq ssBlock2 (ssget "_C" pt1 pt2))
			(command "_.Erase" ssBlock2 "")  
		)
	)
  )	;CB CHANGED FOR FUSE DIRECTORY PLATE 10/4/2021 EMK
  (if (or (= txt "72-200-184-029") (= txt "72-200-183-025")) (setq pt1 '(1.1 4.8 0.0) pt2 '(3.5 4.5 0.0)))					;Operator
  (if (or (= txt "72-200-184-031") (= txt "72-200-183-030")) (setq pt1 '(1.95 3.7 0.0) pt2 '(4.2 3.3 0.0)))					;CT
  (if (or (= txt "72-200-183-003") (= txt "72-200-184-026")) (setq pt1 '(1.35 4.05 0.0) pt2 '(4.0 3.68 0.0)))									;Spec CT
  (if (= txt "72-200-183-009") (setq pt1 '(1.55 5.66 0.0) pt2 '(4.85 5.19 0.0)))								;CT in Spanish
  (setq ssBlock (ssget "_C" pt1 pt2))
  ;(command "PSELECT" ssBlock "")
  (if ssBlock (command "-BHATCH" "PROPERTIES" "SOLID" "CO" "CYAN" "CYAN" "SELECT" ssBlock "" ""))
  (if ssBlock (command "_.Erase" ssBlock ""))
)