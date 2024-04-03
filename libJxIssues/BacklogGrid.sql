SELECT 
    jxIssue.sortOrder As Prio,
    --Typ
    jxIssue.titel As Titel,
    ersteller.name As Ersteller,
    jxIssue.created_at As von,
    jxIssue.prioPunkte As PrioPunkte,
    assignedMA.name AS MA,
    jxIssue.schaetzung As Schätzung,
    jxIssue.deadline As Deadline,
    jxIssue.start As Start

FROM 
    jxIssue

 --Join for the main mitarbeiter (I assumed MA stands for Mitarbeiter Assigned)
INNER JOIN 
    mitarbeiter As assignedMA
ON 
    jxIssue.mitarbeiter_id = assignedMA.id

 --Join for the creator/ersteller
INNER JOIN 
    mitarbeiter as ersteller
ON 
    jxIssue.creator_id = ersteller.id