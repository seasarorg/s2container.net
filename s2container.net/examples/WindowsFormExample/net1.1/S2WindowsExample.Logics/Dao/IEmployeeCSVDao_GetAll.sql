SELECT 
    t_emp.s_code      AS s_code
   ,t_emp.s_name      AS s_name
   ,t_emp.n_gender    AS n_gender
   ,t_emp.d_entry     AS d_entry
   ,t_dept.s_code     AS s_dept_code
   ,t_dept.s_name     AS s_dept_name 
FROM
   t_emp LEFT JOIN t_dept ON t_emp.n_dept_id = t_dept.n_id
ORDER BY t_emp.n_id ASC
