INSERT
INTO    op.ProcessNodeTypes
VALUES  ('GL:Warehouse', 'Warehouse')
,       ('GL:Receiving', 'Receiving')
,       ('GL:Storage',   'Storage')
,       ('GL:Packing',   'Packing')
,       ('GL:Outbound',  'Outbound')

INSERT
INTO    op.ProcessNodes
VALUES  ('Warehouse', 'Warehouse', 'GL:Warehouse',  NULL)
,       ('Receiving', 'Receiving', 'GL:Receiving', 'Warehouse')
,       ('Storage',   'Storage',   'GL:Storage',   'Warehouse')
,       ('Packing',   'Packing',   'GL:Packing',   'Warehouse')
,       ('Outbound',  'Outbound',  'GL:Outbound',  'Warehouse')

INSERT
INTO    op.Methods
        (MethodKey, [Description], IsTransient, StayWithAccessMechanism, StayWithAccessor)
VALUES  ('X:ItemMaster',  'Import Item Master', 1, 0, 0)
,       ('X:CreateOrder', 'Create Order',       1, 0, 0)
,       ('I:ReceiveCase', 'Receive Case',       1, 0, 0)
,       ('I:PutAway',     'Putaway Case',       1, 0, 0)
,       ('F:PutAway',     'Fetch Case',         1, 0, 0)
,       ('D:Pack',        'Packing',            1, 0, 0)
,       ('D:Manifest',    'Manifest',           1, 0, 0)
,       ('D:Ship',        'Shipping',           1, 0, 0)

INSERT
INTO    op.AccessMechanismTypes
        (AccessMechanismTypeKey, [Description], IsUserAccess, IsRoamingAccess, IsPoolable, IsValidatedIPAddress, ProcessNodeTypeKey)
VALUES  ('GL:System',  'System code',                 0, 0, 0, 0, NULL)
,       ('GL:Mobile',  'Mobile device',               1, 1, 0, 0, NULL)
,       ('GL:Station', 'User workstation',            1, 0, 0, 0, NULL)  
,       ('GL:WebUser', 'Web access',                  1, 0, 1, 0, NULL)
,       ('GL:MHE',     'Material handling equipment', 0, 0, 0, 0, NULL)   

INSERT
INTO    op.AccessMechanisms
        (AccessMechanismID, AccessMechanismKey, [Description], ProcessNodeKey, IsEnabled, AccessMechanismTypeKey)
VALUES  (-100, 'SYSTEM',        'System',               'Warehouse', 1, 'GL:System')
,       (-201, 'MOBILE.1',      'Mobile Device 1',      NULL, 1, 'GL:Mobile')
,       (-202, 'MOBILE.2',      'Mobile Device 2',      NULL, 1, 'GL:Mobile')
,       (-301, 'STATION.1',     'Station 1',            NULL, 1, 'GL:Station') -- TODO: zone and position
,       (-302, 'STATION.2',     'Station 2',            NULL, 1, 'GL:Station') -- TODO: zone and position
,       (-401, 'WEB.1',         'Web Virtual Device 1', NULL, 1, 'GL:WebUser')
,       (-402, 'WEB.2',         'Web Virtual Device 2', NULL, 1, 'GL:WebUser')
,       (-501, 'PRINT.APPLY.1', 'Print-apply servicer', NULL, 1, 'GL:MHE')  -- TODO: zone and position

INSERT
INTO    op.AccessorTypes
VALUES  ('GLA:System', 'System Accessor')
,       ('GLA:User',   'User Accessor')
,       ('GLA:MHE',    'Material handling equipment')

INSERT
INTO    op.Accessors
        (AccessorID, AccessorKey, [Description], AccessorTypeKey)
VALUES  (-100, 'SYSTEM',         'System accessor',      'GLA:System')
,       (-201, 'brad@gl.gyro',   'Brad the test user',   'GLA:User')
,       (-202, 'deaner@gl.gyro', 'Deaner the test user', 'GLA:User')
,       (-300, 'PRINT.APPLY',    'Print-apply servicer', 'GLA:MHE')

INSERT
INTO    op.AccessorTypeAccessMechanismTypes
VALUES  ('GLA:System', 'GL:System')
,       ('GLA:User',   'GL:Mobile')
,       ('GLA:User',   'GL:Station')
,       ('GLA:User',   'GL:WebUser')
,       ('GLA:MHE',    'GL:MHE')

INSERT
INTO    op.AccessorCredentialTypes
VALUES  ('HASHWD', 'Hashed Password')
,       ('IPADDR', 'IP Address')
,       ('SECRET', 'Shared Secret')
,       ('TOKCLM', 'Token Claim')

INSERT
INTO    op.AccessorTypeAccessorCredentialTypes
        (AccessorTypeKey, AccessorCredentialTypeKey)
VALUES  ('GLA:User', 'HASHWD')
,       ('GLA:User', 'SECRET')
,       ('GLA:MHE',  'IPADDR')
,       ('GLA:MHE',  'SECRET')
,       ('GLA:User', 'TOKCLM')

insert into op.AccessorCredentials 
values (-202, 'SECRET', 'password', 1)